using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters
{
    public class FacultyParameters : QueryParameters<Faculty>
    {
        public override IQueryable<Faculty> Filter(IQueryable<Faculty> items)
            => items.Search(f => f.Name, SearchTerm);
    }
}
