using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Domain.Interfaces.Shared;

public interface IDbContext : IDisposable
{
    DbContext dbContext { get; }
    bool HasActiveTransaction { get; }
    Task<IDbContextTransaction> BeginTransactionAsync(System.Data.IsolationLevel eTipoTransaccion = System.Data.IsolationLevel.ReadCommitted);
    Task CommitTransactionAsync(IDbContextTransaction transaction);
    Task RollbackTransactionAsync();
    IEnumerable<dynamic> CollectionFromSql(string Sql);
}
