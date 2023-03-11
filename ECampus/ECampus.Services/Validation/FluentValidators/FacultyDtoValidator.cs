using ECampus.Shared.DataTransferObjects;
using FluentValidation;

namespace ECampus.Services.Validation.FluentValidators;

public class FacultyDtoValidator : AbstractValidator<FacultyDto>
{
    public FacultyDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Please, enter name");
    }
}