using UniversityTimetable.Shared.General;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Models
{
    public class Teacher : IIsDeleted, IModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ScienceDegree ScienceDegree { get; set; }

        public bool IsDeleted { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<SubjectTeacher> SubjectIds { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<Class> Classes { get; set; }
    }
}
