using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.Shared.Interfaces.Domain;

public interface IClassService
{
    Task<Timetable> GetTimetableForGroupAsync(int? groupId);
    Task<Timetable> GetTimetableForTeacherAsync(int? teacherId);
    Task<Timetable> GetTimetableForAuditoryAsync(int? auditoryId);
    Task<ValidationResult> ValidateAsync(ClassDto @class);
}