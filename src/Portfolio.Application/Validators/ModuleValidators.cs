using FluentValidation;
using Portfolio.Application.DTOs.Awards;
using Portfolio.Application.DTOs.Certifications;
using Portfolio.Application.DTOs.Documents;
using Portfolio.Application.DTOs.Education;
using Portfolio.Application.DTOs.Projects;
using Portfolio.Application.DTOs.Skills;

namespace Portfolio.Application.Validators;

public sealed class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.ProjectName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).Must((r, e) => !e.HasValue || e >= r.StartDate).WithMessage("End date cannot be before start date.");
    }
}

public sealed class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(x => x.ProjectId).GreaterThan(0);
        RuleFor(x => x.ProjectName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}

public sealed class CreateEducationRequestValidator : AbstractValidator<CreateEducationRequest>
{
    public CreateEducationRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.DegreeLevelId).GreaterThan(0);
        RuleFor(x => x.DegreeId).GreaterThan(0);
        RuleFor(x => x.InstituteId).GreaterThan(0);
        RuleFor(x => x.Gpa).InclusiveBetween(0, 4).When(x => x.Gpa.HasValue);
        RuleFor(x => x.Cgpa).InclusiveBetween(0, 4).When(x => x.Cgpa.HasValue);
    }
}

public sealed class CreateSkillRequestValidator : AbstractValidator<CreateSkillRequest>
{
    public CreateSkillRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.SkillName).NotEmpty().MaximumLength(100);
    }
}

public sealed class CreateAwardRequestValidator : AbstractValidator<CreateAwardRequest>
{
    public CreateAwardRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.AwardName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.StartDate).NotEmpty();
    }
}

public sealed class CreateCertificationRequestValidator : AbstractValidator<CreateCertificationRequest>
{
    public CreateCertificationRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.CertificationIssuerId).GreaterThan(0);
        RuleFor(x => x.CertificationName).NotEmpty().MaximumLength(300);
        RuleFor(x => x.IssueDate).NotEmpty();
    }
}

public sealed class CreateDocumentRequestValidator : AbstractValidator<CreateDocumentRequest>
{
    public CreateDocumentRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.DocumentTypeId).GreaterThan(0);
        RuleFor(x => x.DocumentTitle).NotEmpty().MaximumLength(200);
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(260);
        RuleFor(x => x.StoragePath).NotEmpty().MaximumLength(1000);
    }
}
