using UniversityTimetable.Shared.General;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Models
{
    public class SubjectTeacher : IModel, IIsDeleted
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }

        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
        public bool IsDeleted { get; set; }
    }
}
