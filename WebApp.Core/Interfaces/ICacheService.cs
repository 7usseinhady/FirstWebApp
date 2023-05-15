
namespace WebApp.Core.Interfaces
{
    public interface ICacheService
    {
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expirationTime = null);
        Task<T> GetAsync<T>(string key);
        Task<bool> RemoveAsync(string key);
        Task<bool> RemoveAllStartWithKeyAsync(string key);
        Task<bool> ResetAsync();
    }
}
