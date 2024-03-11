using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using ReadyTechBrewingCoApi.Interfaces;

namespace ReadyTechBrewingCoApi.Wrappers
{
    public class MemoryCacheWrapper : IMemoryCacheWrapper
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheWrapper()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public bool TryGetValue<TItem>(object key, out TItem value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }

        public void Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options)
        {
            _memoryCache.Set(key, value, options);
        }
    }
}