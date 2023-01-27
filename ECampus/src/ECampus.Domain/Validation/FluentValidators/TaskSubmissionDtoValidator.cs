using ECampus.Shared.DataTransferObjects;
using FluentValidation;

namespace ECampus.Domain.Validation.FluentValidators;

public class TaskSubmissionDtoValidator : AbstractValidator<TaskSubmissionDto>
{
    public TaskSubmissionDtoValidator()
    {
        RuleFor(t => t.SubmissionContent)
            .MaximumLength(450)
            .WithMessage("Submission must contain not more than 450 symbols");

        RuleFor(t => t.TotalPoints)
            .LessThanOrEqualTo(t => t.CourseTask!.MaxPoints)
            .When(t => t.CourseTask is not null)
            .WithMessage("Mark cannot be more than maximal mark for this task");

        RuleFor(t => t.TotalPoints)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Mark cannot be less than 0");
    }
}