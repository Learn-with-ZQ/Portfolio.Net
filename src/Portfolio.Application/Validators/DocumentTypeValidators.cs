using FluentValidation;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Validators;

public sealed class CreateDocumentTypeRequestValidator : AbstractValidator<CreateDocumentTypeRequest>
{
    public CreateDocumentTypeRequestValidator()
    {
        RuleFor(x => x.TypeName).NotEmpty().MaximumLength(100);
    }
}

public sealed class UpdateDocumentTypeRequestValidator : AbstractValidator<UpdateDocumentTypeRequest>
{
    public UpdateDocumentTypeRequestValidator()
    {
        RuleFor(x => x.DocumentTypeId).GreaterThan(0);
        RuleFor(x => x.TypeName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}
