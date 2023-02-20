using AutoMapper;
using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Contracts;
using FlowerSpot.SharedKernel.Exceptions;
using MediatR;

namespace FlowerSpot.Application.Features.Commands.AddFlower;
public class AddFlowerCommandHandler : IRequestHandler<AddFlowerCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IProcessingQueue<Flower> _flowerQueue;
    private readonly IMapper _mapper;

    public AddFlowerCommandHandler(IUserRepository userRepository, IProcessingQueue<Flower> flowerQueue, IMapper mapper)
    {
        _userRepository = userRepository;
        _flowerQueue = flowerQueue;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(AddFlowerCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetSingleMatch(x => x.Username == request.Username);

        if (user == null)
        {
            throw new NotFoundException(string.Format(ExceptionMessages.UserNotFound, request.Username));
        }

        var flowerRequest = _mapper.Map<Flower>(request, opt => opt.AfterMap((src, dest) => dest.UserId = user.Id));
        _flowerQueue.Enqueue(flowerRequest);

        return Unit.Value;
    }
}
