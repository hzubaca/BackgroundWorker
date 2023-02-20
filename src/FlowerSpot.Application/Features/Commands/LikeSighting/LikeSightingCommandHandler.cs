using AutoMapper;
using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Contracts;
using FlowerSpot.SharedKernel.Exceptions;
using MediatR;

namespace FlowerSpot.Application.Features.Commands.LikeSighting;
public class LikeSightingCommandHandler : IRequestHandler<LikeSightingCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ISightingRepository _sightingRepository;
    private readonly IProcessingQueue<UserSightingLike> _userSightingLikeQueue;
    private readonly IMapper _mapper;

    public LikeSightingCommandHandler(IUserRepository userRepository, ISightingRepository sightingRepository, IProcessingQueue<UserSightingLike> userSightingLikeQueue, IMapper mapper)
    {
        _userRepository = userRepository;
        _sightingRepository = sightingRepository;
        _userSightingLikeQueue = userSightingLikeQueue;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(LikeSightingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetSingleMatch(x => x.Username == request.Username);

        if (user == null)
        {
            throw new NotFoundException(string.Format(ExceptionMessages.UserNotFound, request.Username));
        }

        var sighting = await _sightingRepository.GetById(request.SightingId);

        if (sighting == null)
        {
            throw new NotFoundException(string.Format(ExceptionMessages.SightingNotFound, request.SightingId));
        }

        _userSightingLikeQueue.Enqueue(_mapper.Map<UserSightingLike>(request, opt => opt.AfterMap((src, dest) => dest.UserId = user.Id)));

        return Unit.Value;
    }
}
