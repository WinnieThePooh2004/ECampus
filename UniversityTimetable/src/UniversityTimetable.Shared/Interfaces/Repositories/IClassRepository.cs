using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Models;
using FluentValidation.Results;
namespace UniversityTimetable.Shared.Interfaces.Repositories
{
    public interface IClassRepository
    {
        Task<TimetableData> GetTimetableForGroupAsync(int groupId);
        Task<TimetableData> GetTimetableForTeacherAsync(int teacherId);
        Task<TimetableData> GetTimetableForAuditoryAsync(int auditoryId);
        Task<Dictionary<string, string>> ValidateAsync(Class @class);
    }
}
