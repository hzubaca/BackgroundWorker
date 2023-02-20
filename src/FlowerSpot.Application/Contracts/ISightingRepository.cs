using FlowerSpot.Domain.Entities;
using FlowerSpot.SharedKernel.Contracts;

namespace FlowerSpot.Application.Contracts;
public interface ISightingRepository : IBaseRepository<Sighting>
{
    List<Sighting> GetAllSightings();
    Task<List<Sighting>> GetAllSightingsAsync();
}
