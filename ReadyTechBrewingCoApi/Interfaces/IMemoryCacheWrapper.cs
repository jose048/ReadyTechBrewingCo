using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ReadyTechBrewingCoApi.Interfaces
{
    public interface IMemoryCacheWrapper
    {
        bool TryGetValue<TItem>(object key, out TItem value);
        void Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options);
    }
}