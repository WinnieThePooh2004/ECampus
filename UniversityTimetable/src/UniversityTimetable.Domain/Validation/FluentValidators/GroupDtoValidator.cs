using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Validation.FluentValidators;

public class GroupDtoValidator : AbstractValidator<GroupDto>
{
    public GroupDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Please, enter name");
    }
}