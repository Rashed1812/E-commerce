using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Contracts
{
    public interface ISpecification<TEntity,T> where TEntity : BaseEntity<T>
    {
        //Create Prop To Get Where Condition in Query
        public Expression<Func<TEntity,bool>> Criteria { get;}
        //Create Prop To Get Order By Condition in Query
        public List<Expression<Func<TEntity,object>>> IncludeExpression { get;}
        //Create Function To Add Sorting Expression
        Expression<Func<TEntity, object>> OrderBy { get; } //Ascending
        Expression<Func<TEntity, object>> OrderByDescending { get; } //Descending
        public int Skip { get; }
        public int Take { get; }
        public bool IsPaginated { get; }
    }
}
