using FlowerSpot.Domain.Resources;
using FluentValidation;

namespace FlowerSpot.Application.Features.Commands.UnlikeSighting;
public class UnlikeSightingCommandValidator : AbstractValidator<UnlikeSightingCommand>
{
    public UnlikeSightingCommandValidator()
    {
        RuleFor(s => s.SightingId).NotEmpty().GreaterThan(0).WithMessage(string.Format(ExceptionMessages.RequiredProperty, "Sighting Id"));
        RuleFor(s => s.Username).NotEmpty().WithMessage(string.Format(ExceptionMessages.RequiredProperty, "Username"));
    }
}
