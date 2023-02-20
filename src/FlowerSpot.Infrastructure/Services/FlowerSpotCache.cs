using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Common;
using FlowerSpot.Domain.Dtos;
using FlowerSpot.Domain.Entities;
using FlowerSpot.SharedKernel.Contracts;

namespace FlowerSpot.Infrastructure.Services;
public class FlowerSpotCache : IFlowerSpotCache
{
    private readonly IDistributedCacheService _distributedCacheService;

    public FlowerSpotCache(IDistributedCacheService distributedCacheService)
    {
        _distributedCacheService = distributedCacheService;
    }

    public async Task<List<SightingDto>?> GetSightings()
    {
        return await _distributedCacheService.Get<List<SightingDto>>(CacheKeys.SIGHTINGS_CACHE_KEY);
    }

    public void AddSightings(List<SightingDto> sightings)
    {
        _distributedCacheService.Add(CacheKeys.SIGHTINGS_CACHE_KEY, sightings, DateTime.Now.AddMinutes(1));
    }

    public async Task<List<UserSightingLike>?> GetSightingLikes(int sightingId)
    {
        var sightings = await _distributedCacheService.Get<List<UserSightingLike>>(CacheKeys.SIGHTINGS_LIKES_CACHE_KEY);
        return sightings?.Where(s => s.SightingId == sightingId).ToList();
    }

    public void AddSightingLikes(List<UserSightingLike> sightingLikes)
    {
        _distributedCacheService.Add(CacheKeys.SIGHTINGS_LIKES_CACHE_KEY, sightingLikes, DateTime.Now.AddMinutes(1));
    }
}
