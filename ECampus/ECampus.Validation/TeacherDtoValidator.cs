using ECampus.Domain.DataTransferObjects;
using FluentValidation;

namespace ECampus.Validation;

public class TeacherDtoValidator : AbstractValidator<TeacherDto>
{
    public TeacherDtoValidator()
    {
        RuleFor(t => t.FirstName)
            .NotEmpty()
            .WithMessage("Please, enter first name");

        RuleFor(t => t.LastName)
            .NotEmpty()
            .WithMessage("Please, enter last name");
    }
}