using ECampus.Shared.DataTransferObjects;
using FluentValidation;

namespace ECampus.Services.Validation.FluentValidators;

public class StudentDtoValidator : AbstractValidator<StudentDto>
{
    public StudentDtoValidator()
    {
        RuleFor(s => s.FirstName)
            .NotEmpty()
            .WithMessage("Please, enter student`s first name");
        
        RuleFor(s => s.LastName)
            .NotEmpty()
            .WithMessage("Please, enter student`s last name");
        
        RuleFor(s => s.GroupId)
            .NotEqual(0)
            .WithMessage("Please, enter student`s group");
    }
}