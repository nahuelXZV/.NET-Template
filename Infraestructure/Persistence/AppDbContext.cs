﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Segurity;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Domain.Interfaces;
using Domain.Entities;
using System.Dynamic;
using System.Data;

namespace Infraestructure.Persistence;

public class AppDbContext : DbContext, IDbContext
{
    private IDbContextTransaction _currentTransaction;
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction != null;
    public DbContext dbContext => this;
    private SqlConnectionStringBuilder _sqlConnectionStringBuilder { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }

    public SqlConnectionStringBuilder getConnectionStringBuilder()
    {
        return new SqlConnectionStringBuilder(this.dbContext.Database.GetConnectionString());
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel eTipoTransaccion = IsolationLevel.ReadCommitted)
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(eTipoTransaccion);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            await _currentTransaction?.RollbackAsync();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public IEnumerable<dynamic> CollectionFromSql(string Sql)
    {
        using (var cmd = this.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText = Sql;
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
            //cmd.Parameters.AddRange(Parameters);
            using (var dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    var dataRow = GetDataRow(dataReader);
                    yield return dataRow;
                }
            }
        }
    }

    public T ExecuteScriptSQL<T>(string sql)
    {
        using (var cmd = this.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText = sql;
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            // cmd.Parameters.AddRange(Parameters); // Si tienes parámetros, agrégalos aquí.

            using (var dataReader = cmd.ExecuteReader())
            {
                if (dataReader.Read())
                {
                    // Si la consulta devuelve una sola columna, retornamos directamente el valor
                    if (typeof(T) != typeof(object) && dataReader.FieldCount == 1)
                    {
                        return (T)dataReader.GetValue(0); // Retornar como tipo genérico
                    }

                    // Si se espera más de un valor, podemos devolver un objeto dinámico
                    var dataRow = GetDataRow(dataReader); // Método similar al que usaste arriba.
                    return (T)(object)dataRow; // Convertir a dinámico
                }
                else
                {
                    return default; // Si no hay resultados
                }
            }
        }
    }


    private dynamic GetDataRow(DbDataReader dataReader)
    {
        var dataRow = new ExpandoObject() as IDictionary<string, object>;
        for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
            dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
        return dataRow;
    }


    #region ENTIDADES
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    #endregion
}
