using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowerSpot.Infrastructure.Repositories;
public class SightingLikesRepository : BaseRepository<UserSightingLike>, ISightingLikesRepository
{
    public SightingLikesRepository(FlowerSpotContext context) : base(context) { }

    public async Task<UserSightingLike?> GetLike(int sightingId, int userId)
    {
        return await _flowerSpotContext.UserSightingLikes.FirstOrDefaultAsync(sl => sl.SightingId == sightingId && sl.UserId == userId);
    }
}
