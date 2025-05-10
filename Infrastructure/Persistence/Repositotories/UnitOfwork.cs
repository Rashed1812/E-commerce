using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Persistence.Data;

namespace Persistence.Repositotories
{
    public class UnitOfwork(StoreDbContext _storeDbContext) : IUnitOfWork
    {
        private readonly Dictionary<string,object> _repositories = [];
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name;

            if (_repositories.ContainsKey(typeName))
            {
                return (IGenericRepository<TEntity, TKey>) _repositories[typeName];
            }
            else
            {
                //Create Object From GenericRepository
                var repo = new GenericRepository<TEntity, TKey>(_storeDbContext);
                //Add to Dictionary
                _repositories.Add(typeName, repo);
                //Return the Object
                return repo;
            }
        }
        public async Task<int> SaveChangesAsync() => await _storeDbContext.SaveChangesAsync();
    }
}
