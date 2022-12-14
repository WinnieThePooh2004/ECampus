using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Validation
{
    public class SubjectDTOValidator : AbstractValidator<SubjectDTO>
    {
        public SubjectDTOValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty()
                .WithMessage("Please, enter name");
        }
    }
}
