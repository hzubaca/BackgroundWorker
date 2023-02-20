using FlowerSpot.Domain.Resources;
using FluentValidation;

namespace FlowerSpot.Application.Features.Commands.DeleteSighting;
public class DeleteSightingCommandValidator : AbstractValidator<DeleteSightingCommand>
{
    public DeleteSightingCommandValidator()
    {
        RuleFor(s => s.Id).NotEmpty().GreaterThan(0).WithMessage(string.Format(ExceptionMessages.RequiredProperty, "Sighting Id"));
        RuleFor(s => s.Username).NotEmpty().WithMessage(string.Format(ExceptionMessages.RequiredProperty, "Username"));
    }
}
