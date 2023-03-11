using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using FluentValidation;

namespace ECampus.Services.Validation.FluentValidators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress()
            .WithMessage("Enter valid email");

        RuleFor(u => u.StudentId)
            .NotNull()
            .When(u => u.Role == UserRole.Student)
            .WithMessage("You must bind user to student to set role as student");
        
        RuleFor(u => u.TeacherId)
            .NotNull()
            .When(u => u.Role == UserRole.Teacher)
            .WithMessage("You must bind user to teacher to set role as teacher");
    }
}