using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using ServiceAbstraction;

namespace Service
{
    public class CasheService(ICasheRepository _casheRepository) : ICasheService
    {
        public async Task<string?> GetAsync(string key)
        {
           return await _casheRepository.GetAsync(key);
        }

        public async Task SetAsync(string CasheKey, object casheValue, TimeSpan timeToLive)
        {
           var value =JsonSerializer.Serialize(casheValue);
           await _casheRepository.SetAsync(CasheKey, value, timeToLive);
        }
    }
}
