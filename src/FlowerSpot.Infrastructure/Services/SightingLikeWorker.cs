using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlowerSpot.Infrastructure.Services;
public class SightingLikeWorker : BackgroundService
{
    private readonly IProcessingQueue<UserSightingLike> _sightingLikeQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SightingLikeWorker> _logger;

    public SightingLikeWorker(IProcessingQueue<UserSightingLike> sightingLikeQueue, IServiceScopeFactory scopeFactory, ILogger<SightingLikeWorker> logger)
    {
        _sightingLikeQueue = sightingLikeQueue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Task will be ran every 0.2 seconds. This is an arbitrary value just for POC.
                await Task.Delay(200, stoppingToken);
                var sightingLike = _sightingLikeQueue.Dequeue();

                if (sightingLike == null) continue;

                // As the worker is a singleton service, we need to create a new repo instance in order to have fresh DB on every run
                using var scope = _scopeFactory.CreateScope();
                var sightingLikesRepository = scope.ServiceProvider.GetRequiredService<ISightingLikesRepository>();
                await sightingLikesRepository.Add(sightingLike);
            }
            catch (Exception ex)
            {
                // Missing dead letter queue
                _logger.LogCritical(ExceptionMessages.FailedUpdatingSightings, ex);
            }
        }
    }
}