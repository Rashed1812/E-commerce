using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositotories
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> specification)
            where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery; //_storeDbContext.Set<TEntity>();

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria); //_storeDbContext.Set<TEntity>().Where(specification.Criteria);
            }
            query = specification.IncludeExpression.Aggregate(query,
                (currentQuery, include) => currentQuery.Include(include));
            //_storeDbContext.Set<TEntity>().Include(specification.IncludeExpression);
            if (specification.OrderBy != null)
                query = query.OrderBy(specification.OrderBy);

            else if (specification.OrderByDescending != null)
                query = query.OrderByDescending(specification.OrderByDescending);

            if (specification.IsPaginated)
                query = query.Skip(specification.Skip).Take(specification.Take);
            return query;
        }
    }
}
