using MediatR;

namespace FlowerSpot.Application.Features.Commands.UnlikeSighting;
public record UnlikeSightingCommand(int SightingId, string Username) : IRequest {}
