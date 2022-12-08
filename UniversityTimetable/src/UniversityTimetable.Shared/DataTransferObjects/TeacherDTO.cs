using UniversityTimetable.Shared.General;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class TeacherDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ScienceDegree ScienceDegree { get; set; }

        public int DepartmentId { get; set; }

        public string FullName => $"{FirstName[0]}. {LastName}";
    }
}
