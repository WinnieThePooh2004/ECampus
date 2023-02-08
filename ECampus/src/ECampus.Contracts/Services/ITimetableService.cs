﻿using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Contracts.Services;

public interface ITimetableService
{
    Task<Timetable> GetTimetableForGroupAsync(int groupId, CancellationToken token = default);
    Task<Timetable> GetTimetableForTeacherAsync(int teacherId, CancellationToken token = default);
    Task<Timetable> GetTimetableForAuditoryAsync(int auditoryId, CancellationToken token = default);
    Task<ValidationResult> ValidateAsync(ClassDto @class, CancellationToken token = default);
}