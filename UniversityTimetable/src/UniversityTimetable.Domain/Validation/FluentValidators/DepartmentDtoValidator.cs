using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Validation.FluentValidators;

public class DepartmentDtoValidator : AbstractValidator<DepartmentDto>
{
    public DepartmentDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Please, enter name");
    }
}