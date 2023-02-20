using FlowerSpot.Domain.Resources;
using FluentValidation;

namespace FlowerSpot.Application.Features.Commands.LogIn;
public class LogInCommandValidator : AbstractValidator<LogInCommand>
{
    public LogInCommandValidator()
    {
        RuleFor(l => l.Username).NotEmpty().MinimumLength(3).MaximumLength(20).WithMessage(ExceptionMessages.UsernameMustContain);
        RuleFor(l => l.Password).NotEmpty().MinimumLength(8).MaximumLength(20).WithMessage(ExceptionMessages.PasswordMustContainLogin);
    }
}
