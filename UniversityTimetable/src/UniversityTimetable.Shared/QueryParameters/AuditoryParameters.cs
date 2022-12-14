using System.Linq.Expressions;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters
{
    public class AuditoryParameters : QueryParameters<Auditory>
    {
        public string AuditoryName { get; set; }
        public string BuildingName { get; set; }

        public override IQueryable<Auditory> Filter(IQueryable<Auditory> items)
            => items.Search(a => a.Name, AuditoryName).Search(a => a.Building, BuildingName);
    }
}
