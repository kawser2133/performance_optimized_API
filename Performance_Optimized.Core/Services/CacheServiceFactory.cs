using Performance_Optimized.Core.Interfaces.IServices;

namespace Performance_Optimized.Core.Services
{
    public class CacheServiceFactory : ICacheServiceFactory
    {
        private readonly ICacheService _memoryCacheService;
        private readonly ICacheService _redisCacheService;

        public CacheServiceFactory(MemoryCacheService memoryCacheService,
            RedisCacheService redisCacheService)
        {
            _memoryCacheService = memoryCacheService;
            _redisCacheService = redisCacheService;
        }

        public ICacheService GetCacheService(CacheType cacheType)
        {
            return cacheType switch
            {
                CacheType.Memory => _memoryCacheService,
                CacheType.Redis => _redisCacheService,
                _ => throw new ArgumentException("Invalid cache type specified")
            };
        }
    }
    public enum CacheType
    {
        Memory,
        Redis
    }

}
