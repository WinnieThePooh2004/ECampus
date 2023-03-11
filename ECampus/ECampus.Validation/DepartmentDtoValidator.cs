using ECampus.Domain.DataTransferObjects;
using FluentValidation;

namespace ECampus.Validation;

public class DepartmentDtoValidator : AbstractValidator<DepartmentDto>
{
    public DepartmentDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Please, enter name");
    }
}