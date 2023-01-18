using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.Shared.Interfaces.Domain;

public interface IUserService
{
    Task<ValidationResult> ValidateCreateAsync(UserDto user);
    Task<ValidationResult> ValidateUpdateAsync(UserDto user);
    Task<PasswordChangeDto> ChangePassword(PasswordChangeDto passwordChange);
    Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange);
}