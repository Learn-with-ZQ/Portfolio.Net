using FluentValidation;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Validators;

public sealed class CreateDegreeLevelRequestValidator : AbstractValidator<CreateDegreeLevelRequest>
{
    public CreateDegreeLevelRequestValidator()
    {
        RuleFor(x => x.DegreeLevelName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DegreePrefix).NotEmpty().MaximumLength(20);
    }
}

public sealed class UpdateDegreeLevelRequestValidator : AbstractValidator<UpdateDegreeLevelRequest>
{
    public UpdateDegreeLevelRequestValidator()
    {
        RuleFor(x => x.DegreeLevelId).GreaterThan(0);
        RuleFor(x => x.DegreeLevelName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DegreePrefix).NotEmpty().MaximumLength(20);
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}
