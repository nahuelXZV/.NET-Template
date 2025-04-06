using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Dynamic;
using System.Data;
using System.Reflection;
using Domain.Interfaces.Shared;

namespace Infraestructure.Persistence;

public class AppDbContext : DbContext, IDbContext, IUnitOfWork
{
    private IDbContextTransaction _currentTransaction;
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction != null;
    public DbContext dbContext => this;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return true;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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

            using (var dataReader = cmd.ExecuteReader())
            {
                if (dataReader.Read())
                {
                    if (typeof(T) != typeof(object) && dataReader.FieldCount == 1)
                    {
                        return (T)dataReader.GetValue(0);
                    }

                    var dataRow = GetDataRow(dataReader);
                    return (T)(object)dataRow;
                }
                else
                {
                    return default;
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
}