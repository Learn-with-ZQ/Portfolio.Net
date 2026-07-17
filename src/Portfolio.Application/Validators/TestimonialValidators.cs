using FluentValidation;
using Portfolio.Application.DTOs.Testimonials;

namespace Portfolio.Application.Validators;

public sealed class SubmitTestimonialRequestValidator : AbstractValidator<SubmitTestimonialRequest>
{
    public SubmitTestimonialRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.AuthorName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AuthorTitle).MaximumLength(200);
        RuleFor(x => x.AuthorCompany).MaximumLength(200);
        RuleFor(x => x.AuthorEmail).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.AuthorEmail));
        RuleFor(x => x.Relationship).MaximumLength(100);
        RuleFor(x => x.Rating).InclusiveBetween(1, 5).When(x => x.Rating.HasValue);
        RuleFor(x => x.Content).NotEmpty().MinimumLength(10).MaximumLength(2000);
        RuleFor(x => x.LinkedInUrl).MaximumLength(500);
    }
}
