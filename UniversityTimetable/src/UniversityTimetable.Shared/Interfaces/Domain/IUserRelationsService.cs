namespace UniversityTimetable.Shared.Interfaces.Domain;

public interface IUserRelationsService
{
    Task SaveAuditory(int userId, int auditoryId);
    Task RemoveSavedAuditory(int userId, int auditoryId);
    Task SaveGroup(int userId, int groupId);
    Task RemoveSavedGroup(int userId, int groupId);
    Task SaveTeacher(int userId, int teacherId);
    Task RemoveSavedTeacher(int userId, int teacherId);
}