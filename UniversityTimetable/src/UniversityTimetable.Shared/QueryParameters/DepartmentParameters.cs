using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters
{
    public class DepartmentParameters : QueryParameters, IQueryParameters<Department>
    {
        public int FacultyId { get; set; }
        public string? DepartmentName { get; set; }
    }
}
