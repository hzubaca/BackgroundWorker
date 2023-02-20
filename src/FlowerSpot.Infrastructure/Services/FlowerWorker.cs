using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlowerSpot.Infrastructure.Services;
public class FlowerWorker : BackgroundService
{
    private readonly IProcessingQueue<Flower> _flowerQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<FlowerWorker> _logger;

    public FlowerWorker(IProcessingQueue<Flower> flowerQueue, IServiceScopeFactory scopeFactory, ILogger<FlowerWorker> logger)
    {
        _flowerQueue = flowerQueue;
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
                var flower = _flowerQueue.Dequeue();

                if (flower == null) continue;

                // As the worker is a singleton service, we need to create a new repo instance in order to have fresh DB on every run
                using var scope = _scopeFactory.CreateScope();
                var flowerRepository = scope.ServiceProvider.GetRequiredService<IFlowerRepository>();
                await flowerRepository.Add(flower);
            }
            catch (Exception ex)
            {
                // Missing dead letter queue
                _logger.LogCritical(ExceptionMessages.FailedUpdatingFlowers, ex);
            }
        }
    }
}