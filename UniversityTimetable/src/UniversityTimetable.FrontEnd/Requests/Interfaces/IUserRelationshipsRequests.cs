namespace UniversityTimetable.FrontEnd.Requests.Interfaces;

public interface IUserRelationshipsRequests
{
    Task SaveAuditory(int auditoryId);
    Task RemoveSavedAuditory(int auditoryId);
    Task SaveGroup(int groupId);
    Task RemoveSavedGroup(int groupId);
    Task SaveTeacher(int teacherId);
    Task RemoveSavedTeacher(int teacherId);
}