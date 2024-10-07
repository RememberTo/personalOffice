using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.Services
{
    internal class CacheManager(ILogger<CacheManager> logger, IDistributedCache cache) : ICacheManager
    {
        private readonly ILogger<CacheManager> _logger = logger;
        private readonly IDistributedCache _cache = cache;

        private int a = 0;

        public async Task<T?> Get<T>(string key) where T : class
        {
            try
            {
                var cachedStr = await _cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(cachedStr)) return JsonConvert.DeserializeObject<T>(cachedStr);

                return null;
            }
            catch (Exception e)
            {
                _logger.LogWarning("Ошибка при получении закеширвоанного объекта: {msg}", e.Message);
                return null;
            }
          
        }

        public async Task Set<T>(string key, T value, TimeSpan expiry = default) where T : class
        {
            try
            {
                if (expiry == default) expiry = TimeSpan.FromMinutes(1);

                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiry
                });
            }
            catch (Exception e)
            {
                _logger.LogWarning("Ошибка при получении закеширвоанного объекта: {msg}", e.Message);
            }  
        }
    }
}
