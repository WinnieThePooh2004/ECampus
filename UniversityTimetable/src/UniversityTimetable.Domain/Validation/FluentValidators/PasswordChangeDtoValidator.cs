using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Validation.FluentValidators;

public class PasswordChangeDtoValidator : AbstractValidator<PasswordChangeDto>
{
    public PasswordChangeDtoValidator()
    {
        RuleFor(p => p.NewPassword)
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 symbols");
        
        RuleFor(p => p.NewPassword)
            .NotEmpty()
            .WithMessage("Enter some password, please.")
            .MinimumLength(8)
            .WithMessage("Password length must be at least 8 characters.")
            .Matches(@"[A-Z]+")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+")
            .WithMessage("Password must contain at least one number.");
        
        RuleFor(p => p.NewPasswordConfirm)
            .Equal(p => p.NewPassword)
            .WithMessage("Passwords don't match");
    }
}