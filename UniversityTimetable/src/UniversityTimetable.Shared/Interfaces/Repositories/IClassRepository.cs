using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.Interfaces.Repositories;

public interface IClassRepository
{
    Task<TimetableData> GetTimetableForGroupAsync(int groupId);
    Task<TimetableData> GetTimetableForTeacherAsync(int teacherId);
    Task<TimetableData> GetTimetableForAuditoryAsync(int auditoryId); 
}