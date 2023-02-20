using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowerSpot.Infrastructure.Repositories;
public class SightingRepository : BaseRepository<Sighting>, ISightingRepository
{
    public SightingRepository(FlowerSpotContext context) : base(context) { }

    public List<Sighting> GetAllSightings()
    {
        return _flowerSpotContext.Sightings.Include(s => s.User)
                                           .Include(s => s.Flower)
                                           .ToList();
    }

    public async Task<List<Sighting>> GetAllSightingsAsync()
    {
        return await _flowerSpotContext.Sightings.Include(s => s.User)
                                                 .Include(s => s.Flower)
                                                 .ToListAsync();
    }
}
