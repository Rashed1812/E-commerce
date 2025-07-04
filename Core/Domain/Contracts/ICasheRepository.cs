﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICasheRepository
    {
        //Get

        Task<string?> GetAsync(string CacheKey);

        //Set
        Task SetAsync(string CacheKey, string CacheValue, TimeSpan? TimeToLive);

    }
}
