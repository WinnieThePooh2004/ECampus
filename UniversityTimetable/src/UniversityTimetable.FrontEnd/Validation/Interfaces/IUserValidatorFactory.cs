using FluentValidation;

namespace UniversityTimetable.FrontEnd.Validation.Interfaces;

public interface IUserValidatorFactory
{
    IValidator<UserDto> CreateValidator();
    IValidator<UserDto> UpdateValidator();
}