using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.Shared.Interfaces.Domain;

public interface IUserService
{
    Task<ValidationResult> ValidateCreateAsync(UserDto user);
    Task<ValidationResult> ValidateUpdateAsync(UserDto user);
    Task<PasswordChangeDto> ChangePassword(PasswordChangeDto passwordChange);
    Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange);
    Task SaveAuditory(int userId, int auditoryId);
    Task RemoveSavedAuditory(int userId, int auditoryId);
    Task SaveGroup(int userId, int groupId);
    Task RemoveSavedGroup(int userId, int groupId);
    Task SaveTeacher(int userId, int teacherId);
    Task RemoveSavedTeacher(int userId, int teacherId);
}