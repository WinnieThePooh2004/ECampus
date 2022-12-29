using UniversityTimetable.Shared.DataContainers;

namespace UniversityTimetable.Shared.Interfaces.DataAccess;

public interface ITimetableDataAccessFacade
{
    Task<TimetableData> GetTimetableForGroupAsync(int groupId);
    Task<TimetableData> GetTimetableForTeacherAsync(int teacherId);
    Task<TimetableData> GetTimetableForAuditoryAsync(int auditoryId);
}