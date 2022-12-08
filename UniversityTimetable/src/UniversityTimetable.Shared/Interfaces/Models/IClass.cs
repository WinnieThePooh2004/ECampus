using UniversityTimetable.Shared.General;

namespace UniversityTimetable.Shared.Interfaces.Models
{
    public interface IClass
    {
        public int Id { get; set; }
        public ClassType ClassType { get; set; }
        public int Number { get; set; }
        public int DayOfTheWeek { get; set; }
        public string SubjectName { get; set; }
        public WeekDependency WeekDependency { get; set; }
        public int TeacherId { get; set; }
        public int GroupId { get; set; }
        public int AuditoryId { get; set; }

    }
}
