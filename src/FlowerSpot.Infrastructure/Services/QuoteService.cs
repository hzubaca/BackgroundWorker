using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Dtos;
using FlowerSpot.SharedKernel.Contracts;
using FlowerSpot.SharedKernel.Services;
using Microsoft.Extensions.Configuration;

namespace FlowerSpot.Infrastructure.Services;
public class QuoteService : IQuoteService
{
    private readonly IFlowerSpotHttpClient _httpClient;

    public QuoteService(IConfiguration configuration)
    {
        _httpClient = new FlowerSpotHttpClient(configuration["ServiceClientUrls:Quotes"]);
    }

    public async Task<QuoteDto?> GetQuoteOfTheDay()
    {
        return await _httpClient.GetAsync<QuoteDto?>("qod?language=en");
    }
}
