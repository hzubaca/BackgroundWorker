using MediatR;

namespace FlowerSpot.Application.Features.Queries.GetFlowers;
public record GetFlowersQuery(DateTime? dateModified = null) : IRequest<GetFlowersResponse>;
