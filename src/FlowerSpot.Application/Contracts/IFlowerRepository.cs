using FlowerSpot.Domain.Entities;
using FlowerSpot.SharedKernel.Contracts;

namespace FlowerSpot.Application.Contracts;
public interface IFlowerRepository : IBaseRepository<Flower>
{
    Task<IReadOnlyCollection<Flower>> GetPage(DateTime? oldestDateModified = null);
}
