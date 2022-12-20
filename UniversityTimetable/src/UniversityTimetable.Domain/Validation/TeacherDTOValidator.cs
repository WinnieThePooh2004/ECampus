using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Validation
{
    public class TeacherDTOValidator : AbstractValidator<TeacherDto>
    {
        public TeacherDTOValidator()
        {
            RuleFor(t => t.FirstName)
                .NotEmpty()
                .WithMessage("Please, enter first name");

            RuleFor(t => t.LastName)
                .NotEmpty()
                .WithMessage("Please, enter last name");
        }
    }
}
