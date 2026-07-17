using FluentValidation;
using Portfolio.Application.DTOs.Experience;

namespace Portfolio.Application.Validators;

public sealed class CreateExperienceRequestValidator : AbstractValidator<CreateExperienceRequest>
{
    public CreateExperienceRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.Designation).NotEmpty().MaximumLength(200);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate)
            .Must((request, endDate) => !endDate.HasValue || endDate.Value >= request.StartDate)
            .WithMessage("End date cannot be before start date.");
    }
}

public sealed class UpdateExperienceRequestValidator : AbstractValidator<UpdateExperienceRequest>
{
    public UpdateExperienceRequestValidator()
    {
        RuleFor(x => x.ExperienceId).GreaterThan(0);
        RuleFor(x => x.Designation).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RowVersion).NotEmpty();
        RuleFor(x => x.EndDate)
            .Must((request, endDate) => !endDate.HasValue || endDate.Value >= request.StartDate)
            .WithMessage("End date cannot be before start date.");
    }
}

public sealed class CreateExperienceDetailRequestValidator : AbstractValidator<CreateExperienceDetailRequest>
{
    public CreateExperienceDetailRequestValidator()
    {
        RuleFor(x => x.ExperienceId).GreaterThan(0);
        RuleFor(x => x.ExperienceDetailName).NotEmpty().MaximumLength(500);
    }
}

public sealed class UpdateExperienceDetailRequestValidator : AbstractValidator<UpdateExperienceDetailRequest>
{
    public UpdateExperienceDetailRequestValidator()
    {
        RuleFor(x => x.ExperienceDetailId).GreaterThan(0);
        RuleFor(x => x.ExperienceDetailName).NotEmpty().MaximumLength(500);
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}

public sealed class SearchExperienceRequestValidator : AbstractValidator<SearchExperienceRequest>
{
    public SearchExperienceRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
    }
}

public sealed class GetExperiencePagedRequestValidator : AbstractValidator<GetExperiencePagedRequest>
{
    public GetExperiencePagedRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
