using ECampus.Domain.DataTransferObjects;
using FluentValidation;

namespace ECampus.Validation;

public class CourseTaskDtoValidator : AbstractValidator<CourseTaskDto>
{
    public CourseTaskDtoValidator()
    {
        RuleFor(c => c.CourseId)
            .NotEqual(0)
            .WithMessage("Student id cannot be 0");

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Task name cannot be empty");

        RuleFor(c => c.Name)
            .MaximumLength(40)
            .WithMessage("Task name cannot be longer than 40 symbols");

        RuleFor(c => c.Coefficient)
            .GreaterThan(0.0)
            .WithMessage("Coefficient must be more than 0")
            .LessThanOrEqualTo(1.0)
            .WithMessage("Coefficient must not be greater than 0");
    }
}