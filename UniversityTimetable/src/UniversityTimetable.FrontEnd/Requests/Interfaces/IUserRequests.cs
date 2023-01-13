namespace UniversityTimetable.FrontEnd.Requests.Interfaces;

public interface IUserRequests : IBaseRequests<UserDto>
{
    Task ChangePassword(PasswordChangeDto passwordChange);
    Task SaveAuditory(int auditoryId);
    Task RemoveSavedAuditory(int auditoryId);
    Task SaveGroup(int groupId);
    Task RemoveSavedGroup(int groupId);
    Task SaveTeacher(int teacherId);
    Task RemoveSavedTeacher(int teacherId);
}