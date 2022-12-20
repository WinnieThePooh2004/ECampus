using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.FrontEnd.Requests.Interfaces;

public interface IUserRequests : IBaseRequests<UserDto>
{
    Task<UserDto> GetCurrentUserAsync();
    Task SaveAuditory(int auditoryId);
    Task RemoveSavedAuditory(int auditoryId);
    Task SaveGroup(int groupId);
    Task RemoveSavedGroup(int groupId);
    Task SaveTeacher(int teacherId);
    Task RemoveSavedTeacher(int teacherId);
}