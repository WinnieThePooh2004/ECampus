using FluentValidation.Results;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Shared.Interfaces.Services;

public interface IClassService : IBaseService<ClassDto>
{
    Task<Timetable> GetTimetableForGroupAsync(int groupId);
    Task<Timetable> GetTimetableForTeacherAsync(int teacherId);
    Task<Timetable> GetTimetableForAuditoryAsync(int auditoryId);
    Task<List<KeyValuePair<string, string>>> ValidateAsync(ClassDto @class);
}