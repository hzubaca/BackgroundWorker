using FlowerSpot.Application.Contracts;
using FlowerSpot.Domain.Resources;
using FluentValidation;

namespace FlowerSpot.Application.Features.Commands.AddSighting;
public class AddSightingCommandValidator : AbstractValidator<AddSightingCommand>
{
    private readonly IFlowerRepository _flowerRepository;

    public AddSightingCommandValidator(IFlowerRepository flowerRepository)
    {
        _flowerRepository = flowerRepository;

        RuleFor(s => s.Username).NotEmpty().WithMessage(string.Format(ExceptionMessages.RequiredProperty, "Username"));
        RuleFor(s => s.FlowerId).NotEmpty().WithMessage(string.Format(ExceptionMessages.RequiredProperty, "Flower Id"));
        RuleFor(s => s.Username).NotEmpty().WithMessage(string.Format(ExceptionMessages.RequiredProperty, "Username"));
        RuleFor(s => s.Longitude).NotEmpty().GreaterThanOrEqualTo(-180).LessThanOrEqualTo(180).WithMessage(string.Format(ExceptionMessages.InvalidCoordinate, "Longitude", "180"));
        RuleFor(s => s.Latitude).NotEmpty().GreaterThanOrEqualTo(-90).LessThanOrEqualTo(90).WithMessage(string.Format(ExceptionMessages.InvalidCoordinate, "Latitude", "180"));

        RuleFor(command => command).CustomAsync((command, context, cancellationToken) => Validate(command, context));
    }

    private async Task Validate(AddSightingCommand command, ValidationContext<AddSightingCommand> validationContext)
    {
        var flower = await _flowerRepository.GetById(command.FlowerId);

        if (flower == null)
        {
            validationContext.AddFailure(string.Format(ExceptionMessages.FlowerNotFound, command.FlowerId));
        }
    }
}
