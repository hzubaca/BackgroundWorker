using FlowerSpot.SharedKernel.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace FlowerSpot.SharedKernel.Services;
public class DistributedCacheService : IDistributedCacheService
{
    private readonly IDistributedCache _flowerSpotCache;

    public DistributedCacheService(IDistributedCache flowerSpotCache)
    {
        _flowerSpotCache = flowerSpotCache;
    }

    public void Add<T>(string key, T value, DateTimeOffset absoluteExpiration)
    {
        var stringValue = JsonConvert.SerializeObject(value);
        _flowerSpotCache.Set(key, Encoding.UTF8.GetBytes(stringValue), new DistributedCacheEntryOptions { AbsoluteExpiration = absoluteExpiration });
    }

    public async Task<T?> Get<T>(string key)
    {
        var bytes = await _flowerSpotCache.GetAsync(key);
        var cachedItem = Encoding.UTF8.GetString(bytes ?? Array.Empty<byte>());
        return JsonConvert.DeserializeObject<T>(cachedItem);
    }
}
