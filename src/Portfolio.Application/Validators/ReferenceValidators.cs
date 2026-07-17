using FluentValidation;
using Portfolio.Application.DTOs.References;

namespace Portfolio.Application.Validators;

public sealed class CreateReferenceRequestValidator : AbstractValidator<CreateReferenceRequest>
{
    public CreateReferenceRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Organization).MaximumLength(200);
        RuleFor(x => x.Designation).MaximumLength(200);
        RuleFor(x => x.Relationship).MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Phone).MaximumLength(50);
        RuleFor(x => x.LinkedInUrl).MaximumLength(500);
    }
}

public sealed class UpdateReferenceRequestValidator : AbstractValidator<UpdateReferenceRequest>
{
    public UpdateReferenceRequestValidator()
    {
        RuleFor(x => x.ReferenceId).GreaterThan(0);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}
