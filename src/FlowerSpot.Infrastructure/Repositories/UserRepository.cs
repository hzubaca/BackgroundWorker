using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Infrastructure.Persistence;

namespace FlowerSpot.Infrastructure.Repositories;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(FlowerSpotContext context) : base(context) { } 
}
