using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Shared.Interfaces.Services;

public interface IUserService : IBaseService<UserDto>
{
    Task<Dictionary<string, string>> ValidateCreateAsync(UserDto user, HttpContext context);
    Task<Dictionary<string, string>> ValidateUpdateAsync(UserDto user);
    Task SaveAuditory(ClaimsPrincipal user, int auditoryId);
    Task RemoveSavedAuditory(ClaimsPrincipal user, int auditoryId);
    Task SaveGroup(ClaimsPrincipal user, int groupId);
    Task RemoveSavedGroup(ClaimsPrincipal user, int groupId);
    Task SaveTeacher(ClaimsPrincipal user, int teacherId);
    Task RemoveSavedTeacher(ClaimsPrincipal user, int teacherId);
}