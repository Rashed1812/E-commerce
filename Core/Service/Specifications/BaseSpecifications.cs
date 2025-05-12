using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;

namespace Service.Specifications
{
    public class BaseSpecifications<TEntity,T> : ISpecification<TEntity,T> where TEntity : BaseEntity<T>
    {
        public BaseSpecifications(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<TEntity, bool>> Criteria
        {
            get;
            private set;
        }
        public List<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];


        public Expression<Func<TEntity, object>> OrderBy 
        {
            get;
            private set;
        }

        public Expression<Func<TEntity, object>> OrderByDescending 
        {
            get;
            private set;
        }

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public bool IsPaginated { get; private set; }

        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            Take = pageSize;
            Skip = (pageIndex-1) * pageSize;
            IsPaginated = true;
        }

        //Create Function To Add Include Expression
        protected void AddIncludes(Expression<Func<TEntity, object>> include)
            => IncludeExpression.Add(include);
        //Create Function To Add Sorting Expression
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderBy)
            => OrderBy = orderBy;
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
            => OrderByDescending = orderByDescending;
    }
}
