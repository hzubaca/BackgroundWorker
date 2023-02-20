using FlowerSpot.Domain.Resources;
using FluentValidation;

namespace FlowerSpot.Application.Features.Commands.AddFlower;
public class AddFlowerCommandValidator : AbstractValidator<AddFlowerCommand>
{
    public AddFlowerCommandValidator()
    {
        RuleFor(l => l.Name).NotEmpty().MaximumLength(50).WithMessage(ExceptionMessages.InvalidFlowerName);
        RuleFor(l => l.ImageRef).NotEmpty().WithMessage(ExceptionMessages.InvalidImageRef);
        RuleFor(l => l.Username).NotEmpty().WithMessage(string.Format(ExceptionMessages.InvalidImageRef, "Username"));
    }
}
