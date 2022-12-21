using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Validation;

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