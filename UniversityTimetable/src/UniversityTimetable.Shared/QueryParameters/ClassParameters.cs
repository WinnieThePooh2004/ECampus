using UniversityTimetable.Shared.General;

namespace UniversityTimetable.Shared.QueryParameters
{
    public class ClassParameters
    {
        public int GroupId { get; set; }
        public int AuditoryId { get; set; }
        public int TeaherId { get; set; }
        public int DayOfWeek { get; set; }
        public int Number { get; set; }
        public ClassType ClassType { get; set; }
    }
}
