using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters
{
    public class GroupParameters : QueryParameters<Group>
    {
        public int DepartmentId { get; set; }

        public override IQueryable<Group> Filter(IQueryable<Group> items)
            => items.Search(g => g.Name, SearchTerm).Where(g => DepartmentId == 0 || g.DepartmentId == DepartmentId);
    }
}
