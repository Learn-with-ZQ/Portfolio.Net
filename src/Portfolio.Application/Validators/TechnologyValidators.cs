using FluentValidation;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Validators;

public sealed class CreateTechnologyRequestValidator : AbstractValidator<CreateTechnologyRequest>
{
    public CreateTechnologyRequestValidator()
    {
        RuleFor(x => x.TechnologyName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Category).MaximumLength(50);
    }
}

public sealed class UpdateTechnologyRequestValidator : AbstractValidator<UpdateTechnologyRequest>
{
    public UpdateTechnologyRequestValidator()
    {
        RuleFor(x => x.TechnologyId).GreaterThan(0);
        RuleFor(x => x.TechnologyName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Category).MaximumLength(50);
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}
