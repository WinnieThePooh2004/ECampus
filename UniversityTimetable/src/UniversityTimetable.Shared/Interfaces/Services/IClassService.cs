using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Pagination;
using UniversityTimetable.Shared.QueryParameters.TimetableParameters;

namespace UniversityTimetable.Shared.Interfaces.Services
{
    public interface IClassService : IBaseService<ClassDTO>
    {
        Task<Timetable<ClassDTO>> GetTimetableForGroupAsync(GroupTimetableParameters parameters);
        Task<Timetable<ClassDTO>> GetTimetableForTeacherAsync(TeacherTimetableParameters parameters);
        Task<Timetable<ClassDTO>> GetTimetableForAuditoryAsync(AuditoryTimetableParameters parameters);
    }
}
