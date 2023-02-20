namespace FlowerSpot.SharedKernel.Contracts;
public interface IFlowerSpotHttpClient
{
    Task<T?> GetAsync<T>(string url);
}
