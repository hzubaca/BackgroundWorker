using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlowerSpot.Application.Features.Commands.LogIn;
public class LogInCommandHandler : IRequestHandler<LogInCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<User> _passwordHasher;

    public LogInCommandHandler(IUserRepository userRepository, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> Handle(LogInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetSingleMatch(x => x.Username == request.Username);

        if (user == null)
        {
            throw new NotFoundException(string.Format(ExceptionMessages.UserNotFound, request.Username));
        }

        var verifyPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

        if (verifyPassword != PasswordVerificationResult.Success)
        {
            throw new UnauthorizedException(ExceptionMessages.WrongPassword);
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username)
        };

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(30), signingCredentials);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }
}
