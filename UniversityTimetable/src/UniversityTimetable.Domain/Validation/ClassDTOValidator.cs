using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Validation
{
    public class ClassDTOValidator : AbstractValidator<ClassDto>
    {
        public ClassDTOValidator()
        {
            RuleFor(c => c.AuditoryId)
                .NotEqual(0)
                .WithMessage("Please, select auditory");

            RuleFor(c => c.GroupId)
                .NotEqual(0)
                .WithMessage("Please, select group");

            RuleFor(c => c.TeacherId)
                .NotEqual(0)
                .WithMessage("Please, select teacher");

            RuleFor(c => c.SubjectId)
                .NotEqual(0)
                .WithMessage("Please, select subject");

        }
    }
}