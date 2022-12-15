using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters
{
    public class DepartmentParameters : QueryParameters, IQueryParameters<Department>
    {
        public int FacultacyId { get; set; }
        public string DepartmentName { get; set; }

        public IQueryable<Department> Filter(IQueryable<Department> items)
            => items.Search(d => d.Name, SearchTerm).Where(d => d.FacultacyId == FacultacyId);
    }
}
