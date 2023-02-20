using MediatR;

namespace FlowerSpot.Application.Features.Commands.LikeSighting;
public record LikeSightingCommand(int SightingId, string Username) : IRequest {}
