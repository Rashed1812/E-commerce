using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace Persistence.Repositotories
{
    public class CasheRepository(IConnectionMultiplexer _connection) : ICasheRepository
    {
        private readonly IDatabase _database = _connection.GetDatabase();
        public async Task<string?> GetAsync(string CacheKey)
        {
            var CacheValue = await _database.StringGetAsync(CacheKey);
            return CacheValue.IsNullOrEmpty ? null : CacheValue.ToString();
        }

        public async Task SetAsync(string CacheKey, string CacheValue, TimeSpan? TimeToLive)
        {
           await _database.StringSetAsync(CacheKey, CacheValue, TimeToLive);
        }
    }
}
