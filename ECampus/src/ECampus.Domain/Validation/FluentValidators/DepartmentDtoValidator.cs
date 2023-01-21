using ECampus.Shared.DataTransferObjects;
using FluentValidation;

namespace ECampus.Domain.Validation.FluentValidators;

public class DepartmentDtoValidator : AbstractValidator<DepartmentDto>
{
    public DepartmentDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Please, enter name");
    }
}