using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Resources;
using FluentValidation;

namespace FlowerSpot.Application.Features.Commands.LogIn;
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(l => l.Username).NotEmpty().MinimumLength(3).MaximumLength(20).WithMessage(ExceptionMessages.UsernameMustContain);
        RuleFor(l => l.Password).NotEmpty().MinimumLength(8).MaximumLength(20).Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$").WithMessage(ExceptionMessages.PasswordMustContain);
        RuleFor(l => l.Email).NotEmpty().Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").WithMessage(string.Format(ExceptionMessages.RequiredProperty, "Email"));

        RuleFor(command => command).CustomAsync((command, context, cancellationToken) => Validate(command, context));
    }

    private async Task Validate(RegisterCommand command, ValidationContext<RegisterCommand> validationContext)
    {
        var user = await _userRepository.GetMultipleMatches(x => x.Username == command.Username);

        if (user.Any())
        {
            validationContext.AddFailure(string.Format(ExceptionMessages.UsernameTaken, command.Username));
        }
    }
}
