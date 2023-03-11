using ECampus.Domain.DataTransferObjects;
using FluentValidation;

namespace ECampus.Services.Validation.FluentValidators;

public class AuditoryDtoValidator : AbstractValidator<AuditoryDto>
{
    public AuditoryDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Please, enter name");

        RuleFor(t => t.Building)
            .NotEmpty()
            .WithMessage("Please, enter building`s name");
    }
}