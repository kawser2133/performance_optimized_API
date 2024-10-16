
namespace Performance_Optimized.Core.Interfaces.IServices
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string cacheKey);
        Task SetAsync<T>(string cacheKey, T value, TimeSpan expirationTime);
        Task RemoveAsync(string cacheKey);
    }
}
