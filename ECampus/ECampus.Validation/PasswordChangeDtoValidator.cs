using ECampus.Domain.DataTransferObjects;
using FluentValidation;

namespace ECampus.Validation;

public class PasswordChangeDtoValidator : AbstractValidator<PasswordChangeDto>
{
    public PasswordChangeDtoValidator()
    {
        RuleFor(p => p.NewPassword)
            .MinimumLength(8)
            .WithMessage("New password must be at least 8 symbols")
            .Matches(@"[A-Z]+")
            .WithMessage("New password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+")
            .WithMessage("New password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+")
            .WithMessage("New password must contain at least one number.");
        
        RuleFor(p => p.NewPasswordConfirm)
            .Equal(p => p.NewPassword)
            .WithMessage("Passwords don't match");
    }
}