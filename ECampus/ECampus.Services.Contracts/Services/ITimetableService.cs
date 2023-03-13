using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Responses.Class;
using ECampus.Domain.Validation;

namespace ECampus.Services.Contracts.Services;

public interface ITimetableService
{
    Task<Timetable> GetTimetableForGroupAsync(int groupId, CancellationToken token = default);
    Task<Timetable> GetTimetableForTeacherAsync(int teacherId, CancellationToken token = default);
    Task<Timetable> GetTimetableForAuditoryAsync(int auditoryId, CancellationToken token = default);
    Task<ValidationResult> ValidateAsync(ClassDto @class, CancellationToken token = default);
}