using FluentValidation;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Validators;

public sealed class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(x => x.CourseName).NotEmpty().MaximumLength(300);
    }
}

public sealed class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseRequest>
{
    public UpdateCourseRequestValidator()
    {
        RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.CourseName).NotEmpty().MaximumLength(300);
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}
