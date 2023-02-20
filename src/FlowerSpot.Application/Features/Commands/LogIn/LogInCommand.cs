using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FlowerSpot.Application.Features.Commands.LogIn;
public class LogInCommand : IRequest<string>
{
    public string Username { get; set; }

    public string Password { get; set; }
}
