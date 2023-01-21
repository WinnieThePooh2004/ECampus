using ECampus.Shared.DataTransferObjects;
using FluentValidation;

namespace ECampus.Domain.Validation.FluentValidators;

public class CourseTaskDtoValidator : AbstractValidator<CourseTaskDto>
{
    public CourseTaskDtoValidator()
    {
        RuleFor(c => c.StudentId)
            .NotEqual(0)
            .WithMessage("Student id cannot be 0");
    }
}