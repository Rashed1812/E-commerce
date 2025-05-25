using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface ICasheService
    {
        Task<string?> GetAsync(string key);

        Task SetAsync(string CasheKey, object casheValue,TimeSpan timeToLive);
    }
}
