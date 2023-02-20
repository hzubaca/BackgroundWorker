using FlowerSpot.Domain.Dtos;

namespace FlowerSpot.Application.Features.Queries.GetFlowers;
public class GetFlowersResponse
{
    public DateTime? OldestDateModified { get; set; }

    public IReadOnlyCollection<FlowerDto> Flowers { get; set; }
}
