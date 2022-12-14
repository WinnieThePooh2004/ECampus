using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Validation
{
    public class ClassDTOValidator : AbstractValidator<ClassDTO>
    {
        public ClassDTOValidator()
        {
            RuleFor(c => c.AuditoryId)
                .NotEqual(0)
                .WithMessage("Please, select some auditory");

            RuleFor(c => c.GroupId)
                .NotEqual(0)
                .WithMessage("Please, select some group");

            RuleFor(c => c.TeacherId)
                .NotEqual(0)
                .WithMessage("Please, select some teacher");

            RuleFor(c => c.SubjectId)
                .NotEqual(0)
                .WithMessage("Please, select some subject");

        }
    }
}