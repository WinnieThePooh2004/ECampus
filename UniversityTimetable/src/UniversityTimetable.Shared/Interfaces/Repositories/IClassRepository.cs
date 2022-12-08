using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Pagination;
using UniversityTimetable.Shared.QueryParameters.TimetableParameters;

namespace UniversityTimetable.Shared.Interfaces.Repositories
{
    public interface IClassRepository : IBaseRepository<Class>
    {
        Task<Timetable<Class>> GetTimetableForGroupAsync(GroupTimetableParameters parameters);
        Task<Timetable<Class>> GetTimetableForTeacherAsync(TeacherTimetableParameters parameters);
        Task<Timetable<Class>> GetTimetableForAuditoryAsync(AuditoryTimetableParameters parameters);
    }
}
