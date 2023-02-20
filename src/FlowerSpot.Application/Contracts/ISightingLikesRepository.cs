using FlowerSpot.Domain.Entities;
using FlowerSpot.SharedKernel.Contracts;

namespace FlowerSpot.Application.Contracts;
public interface ISightingLikesRepository : IBaseRepository<UserSightingLike>
{
    Task<UserSightingLike?> GetLike(int sightingId, int userId);
}
