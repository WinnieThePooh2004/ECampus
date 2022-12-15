using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Models
{
    public class Subject : IIsDeleted, IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public List<Class> Classes { get; set; }
        public List<SubjectTeacher> TeacherIds { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
