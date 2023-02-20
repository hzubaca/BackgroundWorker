using MediatR;

namespace FlowerSpot.Application.Features.Commands.DeleteSighting;
public record DeleteSightingCommand(int Id, string Username) : IRequest {}
