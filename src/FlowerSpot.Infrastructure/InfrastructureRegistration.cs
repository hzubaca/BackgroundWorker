using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Infrastructure.Persistence;
using FlowerSpot.Infrastructure.Repositories;
using FlowerSpot.Infrastructure.Services;
using FlowerSpot.SharedKernel.Contracts;
using FlowerSpot.SharedKernel.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlowerSpot.Infrastructure;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FlowerSpotContext>(o => o.UseNpgsql(configuration.GetConnectionString("FlowerSpotDb")));
        services.AddStackExchangeRedisCache(o => o.Configuration = configuration.GetConnectionString("FlowerSpotCache"));

        services.AddHostedService<FlowerWorker>().AddSingleton<IProcessingQueue<Flower>, ProcessingQueue<Flower>>();
        services.AddHostedService<SightingWorker>().AddSingleton<IProcessingQueue<Sighting>, ProcessingQueue<Sighting>>();
        services.AddHostedService<SightingLikeWorker>().AddSingleton<IProcessingQueue<UserSightingLike>, ProcessingQueue<UserSightingLike>>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFlowerRepository, FlowerRepository>();
        services.AddScoped<ISightingRepository, SightingRepository>();
        services.AddScoped<ISightingLikesRepository, SightingLikesRepository>();
        services.AddScoped<IFlowerSpotCache, FlowerSpotCache>();
        services.AddScoped<IDistributedCacheService, DistributedCacheService>();
        services.AddScoped<IQuoteService, QuoteService>();

        services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

        return services;
    }
}
