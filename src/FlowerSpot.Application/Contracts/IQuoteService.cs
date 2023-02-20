using FlowerSpot.Domain.Dtos;

namespace FlowerSpot.Application.Contracts;
public interface IQuoteService
{
    Task<QuoteDto?> GetQuoteOfTheDay();
}
