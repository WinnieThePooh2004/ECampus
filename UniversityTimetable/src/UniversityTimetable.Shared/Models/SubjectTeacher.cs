using UniversityTimetable.Shared.General;

namespace UniversityTimetable.Shared.Models
{
    public class SubjectTeacher : IIsDeleted
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public bool IsDeleted { get; set; }

        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
    }
}
