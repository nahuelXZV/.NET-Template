using Domain.Common;
using System.Linq.Expressions;

namespace Domain.Interfaces.Shared;

public interface IRepository<EntityType> where EntityType : Entity
{
    IUnitOfWork UnitOfWork { get; }
    Task<EntityType> AddAsync(EntityType entity);
    Task AddRangeAsync(IEnumerable<EntityType> entities);
    void Update(EntityType entity, bool updateRelations = false);
    void UpdateRange(IEnumerable<EntityType> entities, bool updateRelations = false);
    void Delete(EntityType entity, bool softDelete = true);
    void DeleteRange(IEnumerable<EntityType> entities, bool softDelete = true);
    Task<EntityType> GetByIdAsync(long id);
    Task<List<EntityType>> GetAllAsync();
    IQueryable<EntityType> Query();
    IQueryable<OtherType> Query<OtherType>() where OtherType : class;
    IQueryable<EntityType> Include(Expression<Func<EntityType, object>> relation);
    IQueryable<EntityType> Find(Expression<Func<EntityType, bool>> criteria);
}
