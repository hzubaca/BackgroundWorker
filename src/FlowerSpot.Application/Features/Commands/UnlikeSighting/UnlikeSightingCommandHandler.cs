using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Exceptions;
using MediatR;

namespace FlowerSpot.Application.Features.Commands.UnlikeSighting;
public class UnlikeSightingCommandHandler : IRequestHandler<UnlikeSightingCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ISightingLikesRepository _sightingLikesRepository;

    public UnlikeSightingCommandHandler(IUserRepository userRepository, ISightingLikesRepository sightingLikesRepository)
    {
        _userRepository = userRepository;
        _sightingLikesRepository = sightingLikesRepository;
    }

    public async Task<Unit> Handle(UnlikeSightingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetSingleMatch(x => x.Username == request.Username);

        if (user == null)
        {
            throw new NotFoundException(string.Format(ExceptionMessages.UserNotFound, request.Username));
        }

        var sightingLike = await _sightingLikesRepository.GetLike(request.SightingId, user.Id);

        if (sightingLike == null)
        {
            throw new NotFoundException(string.Format(ExceptionMessages.SightingNotFound, request.SightingId));
        }

        if (sightingLike.UserId != user.Id)
        {
            throw new UnauthorizedException(string.Format(ExceptionMessages.MissingPermission, user.Username));
        }

        await _sightingLikesRepository.Delete(sightingLike);

        return Unit.Value;
    }
}
