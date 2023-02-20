using FlowerSpot.Domain.Resources;
using FluentValidation;

namespace FlowerSpot.Application.Features.Queries.GetSighting;
public class GetSightingQueryValidator : AbstractValidator<GetSightingQuery>
{
    public GetSightingQueryValidator()
    {
        RuleFor(s => s.Id).NotEmpty().GreaterThan(0).WithMessage(string.Format(ExceptionMessages.InvalidFlowerName, "Sighting Id"));
    }
}
