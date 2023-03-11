using ECampus.Domain.DataTransferObjects;
using FluentValidation;

namespace ECampus.Services.Validation.FluentValidators;

public class RegistrationDtoValidator : AbstractValidator<RegistrationDto>
{
    public RegistrationDtoValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress()
            .WithMessage("Enter valid email");
        
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
        
        RuleFor(u => u.PasswordConfirm)
            .Equal(x => x.Password)
            .WithMessage("Passwords don't match");
    }
}