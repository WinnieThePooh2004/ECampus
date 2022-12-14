using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters
{
    public class DepartmentParameters : QueryParameters<Department>
    {
        public int FacultacyId { get; set; }
        public string DepartmentName { get; set; }

        public override IQueryable<Department> Filter(IQueryable<Department> items)
            => items.Search(d => d.Name, SearchTerm).Where(d => d.FacultacyId == FacultacyId);
    }
}
