using MediatR;

namespace FlowerSpot.Application.Features.Commands.LogIn;
public class RegisterCommand : IRequest<int>
{
    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }
}
