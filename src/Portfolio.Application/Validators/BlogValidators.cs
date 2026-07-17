using FluentValidation;
using Portfolio.Application.DTOs.Blog;

namespace Portfolio.Application.Validators;

public sealed class CreateBlogPostRequestValidator : AbstractValidator<CreateBlogPostRequest>
{
    public CreateBlogPostRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(300)
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase, hyphen-separated.");
        RuleFor(x => x.ContentMarkdown).NotEmpty();
        RuleFor(x => x.Category).MaximumLength(100);
        RuleFor(x => x.ReadTimeMinutes).GreaterThan(0).When(x => x.ReadTimeMinutes.HasValue);
    }
}

public sealed class UpdateBlogPostRequestValidator : AbstractValidator<UpdateBlogPostRequest>
{
    public UpdateBlogPostRequestValidator()
    {
        RuleFor(x => x.BlogPostId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(300)
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase, hyphen-separated.");
        RuleFor(x => x.ContentMarkdown).NotEmpty();
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}
