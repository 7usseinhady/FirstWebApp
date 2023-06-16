using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Reflection;
using WebApp.Core.Interfaces;

namespace WebApp.Core.Helpers.Caching.Memory
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expirationTime = null)
        {
            try
            {
                var finalExpirationTime = expirationTime ?? TimeSpan.MaxValue;
                _memoryCache.Set(key, value, finalExpirationTime);
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            T value = default!;
            try
            {
                if (_memoryCache.TryGetValue(key, out value!))
                    return await Task.FromResult(value);

                return await Task.FromResult(value);
            }
            catch
            {
                return await Task.FromResult(value);
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                _memoryCache.Remove(key);
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public Task<bool> RemoveAllStartWithKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ResetAsync()
        {
            try
            {
                var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
                var collection = field?.GetValue(_memoryCache) as ICollection;
                if (collection != null)
                    foreach (var item in collection)
                    {
                        var methodInfo = item.GetType().GetProperty("Key");
                        var Key = methodInfo?.GetValue(item);
                        _memoryCache.Remove(Key!);
                    }
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        
    }
}
