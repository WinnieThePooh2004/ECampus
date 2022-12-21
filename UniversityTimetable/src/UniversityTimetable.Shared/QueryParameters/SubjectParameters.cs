using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters
{
    public class SubjectParameters : QueryParameters, IQueryParameters<Subject>
    {
        public int TeacherId { get; set; }
    }
}
