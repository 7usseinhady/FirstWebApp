using StackExchange.Redis;
using WebApp.Core.Interfaces;

namespace WebApp.Core.Helpers.Caching.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _cacheDb;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _cacheDb = _connectionMultiplexer.GetDatabase();
        }


        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expirationTime = null)
        {
            try
            {
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
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

    }
}
