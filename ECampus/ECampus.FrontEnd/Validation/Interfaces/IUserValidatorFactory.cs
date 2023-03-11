using ECampus.Domain.DataTransferObjects;
using FluentValidation;

namespace ECampus.FrontEnd.Validation.Interfaces;

public interface IUserValidatorFactory
{
    IValidator<UserDto> CreateValidator();
    IValidator<UserDto> UpdateValidator();
}