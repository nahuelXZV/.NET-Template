namespace Domain.Interfaces.Shared;

public interface IUnitOfWork
{
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
