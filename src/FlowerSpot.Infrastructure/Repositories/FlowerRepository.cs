using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowerSpot.Infrastructure.Repositories;
public class FlowerRepository : BaseRepository<Flower>, IFlowerRepository
{
    public FlowerRepository(FlowerSpotContext context) : base(context) { }

    public async Task<IReadOnlyCollection<Flower>> GetPage(DateTime? oldestDateModified = null)
    {
        // As ID key is an incremental int, next contingent of 20 flowers will be those of lower ID
        return await _flowerSpotContext.Flowers.Include(f => f.User)
                                               .OrderByDescending(f => f.DateModified)
                                               .Where(f => !oldestDateModified.HasValue || f.DateModified < oldestDateModified)
                                               .Take(20)
                                               .ToListAsync();
    }
}
