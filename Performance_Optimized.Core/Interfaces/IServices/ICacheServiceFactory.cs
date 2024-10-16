
using Performance_Optimized.Core.Services;

namespace Performance_Optimized.Core.Interfaces.IServices
{
    public interface ICacheServiceFactory
    {
        ICacheService GetCacheService(CacheType cacheType);
    }
}
