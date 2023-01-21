using ECampus.Shared.DataTransferObjects;
using FluentValidation;

namespace ECampus.Domain.Validation.FluentValidators;

public class SubjectDtoValidator : AbstractValidator<SubjectDto>
{
    public SubjectDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Please, enter name");
    }
}