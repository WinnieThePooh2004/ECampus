using UniversityTimetable.Shared.General;

namespace UniversityTimetable.Shared.QueryParameters
{
    public class TeacherParameters : QueryParameters
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ScienceDegree ScienceDegree { get; set; }
        public int DepartmentId { get; set; }
    }
}
