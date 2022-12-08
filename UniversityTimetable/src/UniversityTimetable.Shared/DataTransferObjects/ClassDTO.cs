using UniversityTimetable.Shared.General;
using UniversityTimetable.Shared.Interfaces.Models;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class ClassDTO : IClass
    {
        public int Id { get; set; }
        public ClassType ClassType { get; set; }
        public int Number { get; set; }
        public int DayOfTheWeek { get; set; }
        public string SubjectName { get; set; }
        public WeekDependency WeekDependency { get; set; } = WeekDependency.None;

        public int TeacherId { get; set; }
        public int GroupId { get; set; }
        public int AuditoryId { get; set; }
        public AuditoryDTO Auditory { get; set; }
        public TeacherDTO Teacher { get; set; }
        public GroupDTO Group { get; set; }

        public override string ToString()
        {
            return $"ClassDTO:\n" +
                $"DayOfTheWeek:{DayOfTheWeek},\n" +
                $"Number:{Number},\n" +
                $"AuditoName:{Auditory.Name},\n" +
                $"AuditoId:{Auditory.Id},\n" +
                $"GroupName:{Group.Name},\n" +
                $"GroupId:{Group.Id},\n" +
                $"TeacherId:{Teacher.Id}\n";
        }
    }
}
