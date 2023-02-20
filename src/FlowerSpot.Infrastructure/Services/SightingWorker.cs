using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlowerSpot.Infrastructure.Services;
public class SightingWorker : BackgroundService
{
    private readonly IProcessingQueue<Sighting> _sightingQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SightingWorker> _logger;

    public SightingWorker(IProcessingQueue<Sighting> sightingQueue, IServiceScopeFactory scopeFactory, ILogger<SightingWorker> logger)
    {
        _sightingQueue = sightingQueue;
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
                var sighting = _sightingQueue.Dequeue();

                if (sighting == null) continue;

                // As the worker is a singleton service, we need to create a new repo instance in order to have fresh DB on every run
                using var scope = _scopeFactory.CreateScope();
                var sightingRepository = scope.ServiceProvider.GetRequiredService<ISightingRepository>();
                await sightingRepository.Add(sighting);
            }
            catch (Exception ex)
            {
                // Missing dead letter queue
                _logger.LogCritical(ExceptionMessages.FailedUpdatingSightings, ex);
            }
        }
    }
}