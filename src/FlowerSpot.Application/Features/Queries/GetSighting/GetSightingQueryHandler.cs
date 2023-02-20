using AutoMapper;
using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Dtos;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Exceptions;
using MediatR;

namespace FlowerSpot.Application.Features.Queries.GetSighting;
public class GetSightingQueryHandler : IRequestHandler<GetSightingQuery, SightingDto>
{
    private readonly ISightingRepository _sightingCapacity;
    private readonly ISightingLikesRepository _sightingLikesRepository;
    private readonly IFlowerSpotCache _cacheService;
    private readonly IMapper _mapper;
    private static object _lock = new();

    public GetSightingQueryHandler(ISightingRepository sightingRepository, ISightingLikesRepository sightingLikesRepository, IFlowerSpotCache cacheService, IMapper mapper)
    {
        _sightingCapacity = sightingRepository;
        _sightingLikesRepository = sightingLikesRepository;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<SightingDto> Handle(GetSightingQuery request, CancellationToken cancellationToken)
    {
        var cachedSightings = await _cacheService.GetSightings();
        var sighting = cachedSightings?.FirstOrDefault(s => s.SightingId == request.Id);

        if (sighting != null)
        {
            return sighting;
        }

        // If sighting was not find in cache, look for it in db
        // Lock the request as we don't want two concurrent requests retrieving and filling up sighting cache at the same time
        var dbSightings = _sightingCapacity.GetAllSightings();
        sighting = _mapper.Map<SightingDto>(dbSightings.FirstOrDefault(s => s.Id == request.Id));

        if (sighting == null)
        {
            throw new NotFoundException(string.Format(ExceptionMessages.SightingNotFound, request.Id));
        }

        // Same process for likes
        var sightingLikes = await _cacheService.GetSightingLikes(request.Id);

        if (sightingLikes?.Any() ?? false)
        {
            sighting.LikesCount = sightingLikes.Count;
            return sighting;
        }

        var dbSightingsLikes = await _sightingLikesRepository.GetAll();

        lock (_lock)
        {
            _cacheService.AddSightingLikes(dbSightingsLikes.ToList());
            sighting.LikesCount = dbSightingsLikes?.Count(x => x.SightingId == request.Id) ?? 0;

            _cacheService.AddSightings(dbSightings.Select(s => _mapper.Map<SightingDto>(s, opt => opt.AfterMap((src, dest) => 
            {
                dest.LikesCount = dbSightingsLikes?.Count(sl => sl.SightingId == s.Id) ?? 0;
            }))).ToList());
        }

        return sighting;
    }
}
