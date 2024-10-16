using Performance_Optimized.Core.Interfaces.IServices;
using StackExchange.Redis;
using System.Text.Json;

namespace Performance_Optimized.Core.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _redisDatabase;

        public RedisCacheService(string connectionString)
        {
            var redis = ConnectionMultiplexer.Connect(connectionString);
            _redisDatabase = redis.GetDatabase();
        }

        public async Task<T> GetAsync<T>(string cacheKey)
        {
            var cachedValue = await _redisDatabase.StringGetAsync(cacheKey);
            if (cachedValue.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(cachedValue);
        }

        public async Task SetAsync<T>(string cacheKey, T value, TimeSpan expirationTime)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await _redisDatabase.StringSetAsync(cacheKey, serializedValue, expirationTime);
        }

        public async Task RemoveAsync(string cacheKey)
        {
            await _redisDatabase.KeyDeleteAsync(cacheKey);
        }
    }

}
