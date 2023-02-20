using FlowerSpot.Domain.Dtos;

namespace FlowerSpot.Application.Features.Queries.GetSighting;
public class GetSightingResponse
{
    public IReadOnlyCollection<SightingDto> Sightings { get; set; }
}
