using AutoMapper;
using FlowerSpot.Application.Features.Commands.AddFlower;
using FlowerSpot.Application.Features.Commands.AddSighting;
using FlowerSpot.Application.Features.Commands.LikeSighting;
using FlowerSpot.Application.Features.Commands.LogIn;
using FlowerSpot.Application.Features.Queries.GetFlowers;
using FlowerSpot.Domain.Dtos;
using FlowerSpot.Domain.Entities;

namespace FlowerSpot.Application;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RegisterCommand, User>();
        CreateMap<AddFlowerCommand, Flower>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.DateModified, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<IReadOnlyCollection<Flower>, GetFlowersResponse>()
            .ForMember(dest => dest.Flowers, opt => opt.MapFrom(x => x.Select(y => new FlowerDto
            {
                Id = y.Id,
                Name = y.Name,
                Username = y.User.Username,
                Description = y.Description,
                DateModified = y.DateModified
            })))
            .ForMember(dest => dest.OldestDateModified, opt => opt.MapFrom(x => x != null && x.Any() ? x.Min(y => y.DateModified) : new DateTime()));

        CreateMap<AddSightingCommand, Sighting>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Flower, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Quote, opt => opt.Ignore());

        CreateMap<Sighting, SightingDto>()
            .ForMember(dest => dest.SightingId, opt => opt.MapFrom(x => x.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(x => x.User.Username))
            .ForMember(dest => dest.FlowerName, opt => opt.MapFrom(x => x.Flower.Name))
            .ForMember(dest => dest.LikesCount, opt => opt.Ignore());

        CreateMap<LikeSightingCommand, UserSightingLike>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
    }
}
