using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Shared.Interfaces.Domain;

public interface IUserService : IBaseService<UserDto>
{
    Task<List<KeyValuePair<string, string>>> ValidateCreateAsync(UserDto user);
    Task<List<KeyValuePair<string, string>>> ValidateUpdateAsync(UserDto user);
    Task<PasswordChangeDto> ChangePassword(PasswordChangeDto passwordChange);
    Task<List<KeyValuePair<string, string>>> ValidatePasswordChange(PasswordChangeDto passwordChange);
    Task SaveAuditory(int userId, int auditoryId);
    Task RemoveSavedAuditory(int userId, int auditoryId);
    Task SaveGroup(int userId, int groupId);
    Task RemoveSavedGroup(int userId, int groupId);
    Task SaveTeacher(int userId, int teacherId);
    Task RemoveSavedTeacher(int userId, int teacherId);
}