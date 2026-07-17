using FluentValidation;
using Portfolio.Application.DTOs.Analytics;

namespace Portfolio.Application.Validators;

public sealed class TrackEventRequestValidator : AbstractValidator<TrackEventRequest>
{
    private static readonly string[] AllowedTypes =
    {
        "PageView", "Visit", "ResumeDownload", "CertificateDownload",
        "ProjectView", "BlogView", "ContactRequest", "DocumentDownload"
    };

    public TrackEventRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.EventType).NotEmpty().MaximumLength(50)
            .Must(t => AllowedTypes.Contains(t)).WithMessage("Unknown event type.");
        RuleFor(x => x.Path).MaximumLength(500);
        RuleFor(x => x.VisitorId).MaximumLength(64);
        RuleFor(x => x.Country).MaximumLength(100);
        RuleFor(x => x.City).MaximumLength(100);
        RuleFor(x => x.Browser).MaximumLength(100);
        RuleFor(x => x.Device).MaximumLength(50);
        RuleFor(x => x.Referrer).MaximumLength(500);
    }
}
