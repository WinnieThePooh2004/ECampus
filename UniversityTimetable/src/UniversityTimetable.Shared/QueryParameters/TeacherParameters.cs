using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters
{
    public class TeacherParameters : QueryParameters, IQueryParameters<Teacher>
    {
        public int DepartmentId { get; set; }

        public IQueryable<Teacher> Filter(IQueryable<Teacher> items)
            => items.Search(g => g.LastName, SearchTerm).Where(t => DepartmentId == 0 || t.DepartmentId == DepartmentId);

    }
}
