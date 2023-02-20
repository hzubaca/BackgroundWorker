using AutoMapper;
using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace FlowerSpot.Application.Features.Commands.LogIn;
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(IUserRepository userRepository, IConfiguration configuration, IPasswordHasher<User> passwordHasher, IMapper mapper)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        var hashedPassword = _passwordHasher.HashPassword(user, request.Password);
        user.Password = hashedPassword;

        var addedUser = await _userRepository.Add(user);

        return addedUser.Id;
    }
}
