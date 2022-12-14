using FluentValidation;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Validation
{
    public class FacultyDTOValidator : AbstractValidator<Faculty>
    {
        public FacultyDTOValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty()
                .WithMessage("Please, enter name");
        }
    }
}
