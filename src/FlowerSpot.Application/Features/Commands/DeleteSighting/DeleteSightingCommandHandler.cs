using AutoMapper;
using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Dtos;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Exceptions;
using MediatR;

namespace FlowerSpot.Application.Features.Commands.DeleteSighting;
public class DeleteSightingCommandHandler : IRequestHandler<DeleteSightingCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ISightingRepository _sightingRepository;
    private readonly IFlowerSpotCache _cacheService;
    private readonly IMapper _mapper;
    private static object _lock = new();

    public DeleteSightingCommandHandler(IUserRepository userRepository, ISightingRepository sightingRepository, IFlowerSpotCache cacheService, IMapper mapper)
    {
        _userRepository = userRepository;
        _sightingRepository = sightingRepository;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteSightingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetSingleMatch(x => x.Username == request.Username);

        if (user == null)
        {
            throw new NotFoundException(string.Format(ExceptionMessages.UserNotFound, request.Username));
        }

        var sighting = await _sightingRepository.GetById(request.Id);

        if (sighting == null)
        {
            throw new NotFoundException(string.Format(ExceptionMessages.SightingNotFound, request.Id));
        }

        if (sighting.UserId != user.Id)
        {
            throw new UnauthorizedException(string.Format(ExceptionMessages.MissingPermission, user.Username));
        }

        await _sightingRepository.Delete(sighting);

        var sightings = await _sightingRepository.GetAllSightingsAsync();

        lock (_lock)
        {
            _cacheService.AddSightings(sightings.Select(s => _mapper.Map<SightingDto>(s)).ToList());
        }

        return Unit.Value;
    }
}
