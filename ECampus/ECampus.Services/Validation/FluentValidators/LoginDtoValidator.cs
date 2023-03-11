using ECampus.Domain.DataTransferObjects;
using FluentValidation;

namespace ECampus.Services.Validation.FluentValidators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(l => l.Email)
            .EmailAddress();
        
        RuleFor(u => u.Password)
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 symbols");
        
        RuleFor(u => u.Password)
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
    }
}