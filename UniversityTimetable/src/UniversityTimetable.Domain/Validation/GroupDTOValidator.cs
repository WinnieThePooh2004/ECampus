using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Validation
{
    public class GroupDTOValidator : AbstractValidator<GroupDto>
    {
        public GroupDTOValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty()
                .WithMessage("Please, enter name");
        }
    }
}
