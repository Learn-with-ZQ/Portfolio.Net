using FluentValidation;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Validators;

public sealed class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator()
    {
        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.WebsiteUrl).MaximumLength(500);
    }
}

public sealed class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
{
    public UpdateCompanyRequestValidator()
    {
        RuleFor(x => x.CompanyId).GreaterThan(0);
        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.WebsiteUrl).MaximumLength(500);
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}

public sealed class CreateDeployDetailRequestValidator : AbstractValidator<CreateDeployDetailRequest>
{
    public CreateDeployDetailRequestValidator()
    {
        RuleFor(x => x.DeployDetailName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DeployedTo).NotEmpty().MaximumLength(200);
    }
}

public sealed class UpdateDeployDetailRequestValidator : AbstractValidator<UpdateDeployDetailRequest>
{
    public UpdateDeployDetailRequestValidator()
    {
        RuleFor(x => x.DeployDetailId).GreaterThan(0);
        RuleFor(x => x.DeployDetailName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DeployedTo).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}
