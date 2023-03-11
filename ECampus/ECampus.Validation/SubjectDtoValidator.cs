using ECampus.Domain.DataTransferObjects;
using FluentValidation;

namespace ECampus.Validation;

public class SubjectDtoValidator : AbstractValidator<SubjectDto>
{
    public SubjectDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Please, enter name");
    }
}