using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        //Add
        Task AddAsync(TEntity entity);
        //Get All
        Task<IEnumerable<TEntity>> GetAllAsync();
        //Get All With Specification
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification);
        //Get By Id
        Task<TEntity> GetByIdAsync(TKey id);
        //Get By Id With Specification
        Task<TEntity> GetByIdAsync(ISpecification<TEntity, TKey> specification);
        //Update
        void Update(TEntity entity);
        //Remove
        void Remove(TEntity entity);
        Task<int> CountAsync(ISpecification<TEntity, TKey> specification);
    }
}
