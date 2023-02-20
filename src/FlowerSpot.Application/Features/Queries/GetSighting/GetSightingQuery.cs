using FlowerSpot.Domain.Dtos;
using MediatR;

namespace FlowerSpot.Application.Features.Queries.GetSighting;
public record GetSightingQuery(int Id) : IRequest<SightingDto>;
