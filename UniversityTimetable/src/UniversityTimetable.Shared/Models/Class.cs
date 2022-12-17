using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Models
{
    public class Class : IIsDeleted, IModel
    {
        public int Id { get; set; }
        public ClassType ClassType { get; set; }
        public int Number { get; set; }
        public int DayOfWeek { get; set; }
        public bool IsDeleted { get; set; }
        public int SubjectId { get; set; }
        public WeekDependency WeekDependency { get; set; } = WeekDependency.None;
        
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null;
        public int GroupId { get; set; }
        public Group Group { get; set; } = null;
        public int AuditoryId { get; set; }
        public Auditory Auditory { get; set; } = null;
        public Subject Subject { get; set; }
    }
}
