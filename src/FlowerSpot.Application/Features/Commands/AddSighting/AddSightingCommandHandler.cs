using AutoMapper;
using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Contracts;
using FlowerSpot.SharedKernel.Exceptions;
using MediatR;

namespace FlowerSpot.Application.Features.Commands.AddSighting;
public class AddSightingCommandHandler : IRequestHandler<AddSightingCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IProcessingQueue<Sighting> _sightingQueue;
    private readonly IQuoteService _quoteService;
    private readonly IMapper _mapper;

    public AddSightingCommandHandler(IUserRepository userRepository, IProcessingQueue<Sighting> sightingQueue, IQuoteService quoteService, IMapper mapper)
    {
        _userRepository = userRepository;
        _sightingQueue = sightingQueue;
        _quoteService = quoteService;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(AddSightingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetSingleMatch(x => x.Username == request.Username);

        if (user == null)
        {
            throw new NotFoundException(string.Format(ExceptionMessages.UserNotFound, request.Username));
        }

        var quoteOfTheDay = await _quoteService.GetQuoteOfTheDay();

        var sightingRequest = _mapper.Map<Sighting>(request, opt => opt.AfterMap((src, dest) =>
        {
            dest.UserId = user.Id;
            dest.Quote = quoteOfTheDay?.Contents?.Quotes?.FirstOrDefault()?.Quote;
        }));

        // Queue the request so the DB does not get overloaded, and user does not wait for the process to complete.
        _sightingQueue.Enqueue(sightingRequest);

        return Unit.Value;
    }
}
