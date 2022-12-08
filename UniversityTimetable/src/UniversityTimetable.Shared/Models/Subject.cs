using UniversityTimetable.Shared.General;

namespace UniversityTimetable.Shared.Models
{
    public class Subject : IIsDeleted
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public List<Class> Classes { get; set; }
        public List<SubjectTeacher> Teachers { get; set; }
    }
}
