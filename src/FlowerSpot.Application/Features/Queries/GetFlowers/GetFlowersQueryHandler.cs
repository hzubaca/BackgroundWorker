using AutoMapper;
using FlowerSpot.Application.Contracts;
using MediatR;

namespace FlowerSpot.Application.Features.Queries.GetFlowers;
public class GetFlowersQueryHandler : IRequestHandler<GetFlowersQuery, GetFlowersResponse>
{
    private readonly IFlowerRepository _flowerRepository;
    private readonly IMapper _mapper;

    public GetFlowersQueryHandler(IFlowerRepository flowerRepository, IMapper mapper)
    {
        _flowerRepository = flowerRepository;
        _mapper = mapper;
    }

    public async Task<GetFlowersResponse> Handle(GetFlowersQuery request, CancellationToken cancellationToken)
    {
        var flowers = await _flowerRepository.GetPage(request.dateModified);

        return _mapper.Map<GetFlowersResponse>(flowers);
    }
}
