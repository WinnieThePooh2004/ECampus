using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Shared.Interfaces.Domain;

public interface IClassService
{
    Task<Timetable> GetTimetableForGroupAsync(int? groupId);
    Task<Timetable> GetTimetableForTeacherAsync(int? teacherId);
    Task<Timetable> GetTimetableForAuditoryAsync(int? auditoryId);
    Task<ValidationResult> ValidateAsync(ClassDto @class);
}