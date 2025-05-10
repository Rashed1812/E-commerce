using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositotories
{
    public class GenericRepository<TEntity, TKey>(StoreDbContext _storeDbContext) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public async Task AddAsync(TEntity entity) => await _storeDbContext.Set<TEntity>().AddAsync(entity);
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _storeDbContext.Set<TEntity>().ToListAsync();
        public async Task<TEntity?> GetByIdAsync(TKey id) => await _storeDbContext.Set<TEntity>().FindAsync(id);
        public void Remove(TEntity entity) => _storeDbContext.Set<TEntity>().Remove(entity);
        public void Update(TEntity entity) => _storeDbContext.Set<TEntity>().Update(entity);
    }
}
