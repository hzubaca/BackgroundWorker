namespace FlowerSpot.SharedKernel.Contracts;
public interface IDistributedCacheService
{
    void Add<T>(string key, T value, DateTimeOffset absoluteExpiration);
    Task<T?> Get<T>(string key);
}
