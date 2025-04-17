using System.Linq.Expressions;
using Domain.DTOs.Shared;

namespace Domain.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> ApplyFilter<TEntity>(this IQueryable<TEntity> query,
        FilterDTO? filter, Expression<Func<TEntity, bool>>? searchExpression = null)
        where TEntity : class
    {
        if (filter == null) return query;

        if (!string.IsNullOrWhiteSpace(filter.Search) && searchExpression != null)
        {
            query = query.Where(searchExpression);
        }

        if (filter.Offset > 0)
        {
            query = query.Skip(filter.Offset);
        }

        if (filter.Limit > 0)
        {
            query = query.Take(filter.Limit);
        }

        return query;
    }
}

