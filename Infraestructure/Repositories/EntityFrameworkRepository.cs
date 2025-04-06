using Microsoft.EntityFrameworkCore;
using Infraestructure.Persistence;
using System.Linq.Expressions;
using Domain.Common;
using Domain.Interfaces.Shared;

namespace Infraestructure.Repositories;

public class EntityFrameworkRepository<EntityType> : IRepository<EntityType> where EntityType : Entity
{
    private readonly AppDbContext _context;

    public EntityFrameworkRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IUnitOfWork UnitOfWork => _context;
    public async Task<EntityType> AddAsync(EntityType entity)
    {
        return (await _context.Set<EntityType>().AddAsync(entity)).Entity;
    }

    public async Task AddRangeAsync(IEnumerable<EntityType> entities)
    {
        await _context.Set<EntityType>().AddRangeAsync(entities);
    }

    public void Update(EntityType entity, bool updateRelations = false)
    {
        if (updateRelations)
        {
            _context.Set<EntityType>().Update(entity);
        }
        else
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }

    public void UpdateRange(IEnumerable<EntityType> entities, bool updateRelations = false)
    {
        if (updateRelations)
        {
            _context.Set<EntityType>().UpdateRange(entities);
        }
        else
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
        }
    }

    public void Delete(EntityType entity, bool softDelete = true)
    {
        if (softDelete)
        {
            Update(entity);
            entity.Eliminado = true;
        }
        else
        {
            _context.Set<EntityType>().Remove(entity);
        }
    }
    public void DeleteRange(IEnumerable<EntityType> entities, bool softDelete = true)
    {
        if (softDelete)
        {
            UpdateRange(entities);
            foreach (var entity in entities)
            {
                entity.Eliminado = true;
            }
        }
        else
        {
            _context.Set<EntityType>().RemoveRange(entities);
        }
    }

    public IQueryable<EntityType> Find(Expression<Func<EntityType, bool>> criteria)
    {
        return _context.Set<EntityType>().Where(criteria);
    }

    public async Task<List<EntityType>> GetAllAsync()
    {
        return await _context.Set<EntityType>().ToListAsync();
    }

    public async Task<EntityType> GetByIdAsync(long id)
    {
        return await _context.Set<EntityType>().FindAsync(id);
    }

    public IQueryable<EntityType> Include(Expression<Func<EntityType, object>> relation)
    {
        return _context.Set<EntityType>().Include(relation);
    }

    public IQueryable<EntityType> Query()
    {
        return _context.Set<EntityType>();
    }

    public IQueryable<OtherType> Query<OtherType>() where OtherType : class
    {
        return _context.Set<OtherType>();
    }
}
