using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Shared.Interfaces.Services;

public interface IUserService : IBaseService<UserDto>
{
    Task<List<KeyValuePair<string, string>>> ValidateCreateAsync(UserDto user, HttpContext context);
    Task<List<KeyValuePair<string, string>>> ValidateUpdateAsync(UserDto user);
    Task<UserDto> ChangePassword(PasswordChangeDto passwordChange);
    Task<List<KeyValuePair<string, string>>> ValidatePasswordChange(PasswordChangeDto passwordChange);
    Task SaveAuditory(ClaimsPrincipal user, int auditoryId);
    Task RemoveSavedAuditory(ClaimsPrincipal user, int auditoryId);
    Task SaveGroup(ClaimsPrincipal user, int groupId);
    Task RemoveSavedGroup(ClaimsPrincipal user, int groupId);
    Task SaveTeacher(ClaimsPrincipal user, int teacherId);
    Task RemoveSavedTeacher(ClaimsPrincipal user, int teacherId);
}