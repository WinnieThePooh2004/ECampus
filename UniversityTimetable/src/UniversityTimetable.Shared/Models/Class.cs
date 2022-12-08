using UniversityTimetable.Shared.General;
using UniversityTimetable.Shared.Interfaces.Models;

namespace UniversityTimetable.Shared.Models
{
    public class Class : IIsDeleted, IClass
    {
        public int Id { get; set; }
        public ClassType ClassType { get; set; }
        public int Number { get; set; }
        public int DayOfTheWeek { get; set; }
        public bool IsDeleted { get; set; }
        public string SubjectName { get; set; }
        public WeekDependency WeekDependency { get; set; } = WeekDependency.None;

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int AuditoryId { get; set; }
        public Auditory Auditory { get; set; }
    }
}
