using FlowerSpot.Domain.Dtos;
using FlowerSpot.Domain.Entities;

namespace FlowerSpot.Application.Contracts;
public interface IFlowerSpotCache
{
    Task<List<SightingDto>?> GetSightings();
    void AddSightings(List<SightingDto> sightings);
    Task<List<UserSightingLike>?> GetSightingLikes(int sightingId);
    void AddSightingLikes(List<UserSightingLike> sightingLikes);
}
