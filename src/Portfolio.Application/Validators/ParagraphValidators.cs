using FluentValidation;
using Portfolio.Application.DTOs.Paragraphs;

namespace Portfolio.Application.Validators;

public sealed class CreateParagraphRequestValidator : AbstractValidator<CreateParagraphRequest>
{
    public CreateParagraphRequestValidator()
    {
        RuleFor(x => x.PortfolioProfileId).GreaterThan(0);
        RuleFor(x => x.ParagraphTypeId).GreaterThan(0);
        RuleFor(x => x.ParagraphTitle).MaximumLength(200);
        RuleFor(x => x.ParagraphText).NotEmpty();
    }
}

public sealed class UpdateParagraphRequestValidator : AbstractValidator<UpdateParagraphRequest>
{
    public UpdateParagraphRequestValidator()
    {
        RuleFor(x => x.ParagraphId).GreaterThan(0);
        RuleFor(x => x.ParagraphTypeId).GreaterThan(0);
        RuleFor(x => x.ParagraphTitle).MaximumLength(200);
        RuleFor(x => x.ParagraphText).NotEmpty();
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}
