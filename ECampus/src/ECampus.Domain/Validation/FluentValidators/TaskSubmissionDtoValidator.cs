using ECampus.Shared.DataTransferObjects;
using FluentValidation;

namespace ECampus.Domain.Validation.FluentValidators;

public class TaskSubmissionDtoValidator : AbstractValidator<TaskSubmissionDto>
{
    public TaskSubmissionDtoValidator()
    {
        RuleFor(t => t.SubmissionContent)
            .MinimumLength(20)
            .WithMessage("Submission must contain at least 20 symbols");

        RuleFor(t => t.SubmissionContent)
            .MaximumLength(450)
            .WithMessage("Submission must contain not more than 450 symbols");
    }
}