using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters
{
    public class AuditoryParameters : QueryParameters, IQueryParameters<Auditory>
    {
        public string AuditoryName { get; set; }
        public string BuildingName { get; set; }
    }
}
