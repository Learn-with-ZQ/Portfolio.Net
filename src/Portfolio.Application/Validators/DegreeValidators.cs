using FluentValidation;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Validators;

public sealed class CreateDegreeRequestValidator : AbstractValidator<CreateDegreeRequest>
{
    public CreateDegreeRequestValidator()
    {
        RuleFor(x => x.DegreeName).NotEmpty().MaximumLength(200);
    }
}

public sealed class UpdateDegreeRequestValidator : AbstractValidator<UpdateDegreeRequest>
{
    public UpdateDegreeRequestValidator()
    {
        RuleFor(x => x.DegreeId).GreaterThan(0);
        RuleFor(x => x.DegreeName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}
