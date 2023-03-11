using ECampus.Shared.DataTransferObjects;
using FluentValidation;

namespace ECampus.Services.Validation.FluentValidators;

public class GroupDtoValidator : AbstractValidator<GroupDto>
{
    public GroupDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Please, enter name");
    }
}