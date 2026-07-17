using FluentValidation;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Validators;

public sealed class CreateCertificationIssuerRequestValidator : AbstractValidator<CreateCertificationIssuerRequest>
{
    public CreateCertificationIssuerRequestValidator()
    {
        RuleFor(x => x.IssuerName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.IssuerWebsite).MaximumLength(500);
    }
}

public sealed class UpdateCertificationIssuerRequestValidator : AbstractValidator<UpdateCertificationIssuerRequest>
{
    public UpdateCertificationIssuerRequestValidator()
    {
        RuleFor(x => x.CertificationIssuerId).GreaterThan(0);
        RuleFor(x => x.IssuerName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.IssuerWebsite).MaximumLength(500);
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}
