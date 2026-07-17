using FluentValidation;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Validators;

public sealed class CreateInstituteRequestValidator : AbstractValidator<CreateInstituteRequest>
{
    public CreateInstituteRequestValidator()
    {
        RuleFor(x => x.InstituteName).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Location).MaximumLength(300);
    }
}

public sealed class UpdateInstituteRequestValidator : AbstractValidator<UpdateInstituteRequest>
{
    public UpdateInstituteRequestValidator()
    {
        RuleFor(x => x.InstituteId).GreaterThan(0);
        RuleFor(x => x.InstituteName).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Location).MaximumLength(300);
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}
