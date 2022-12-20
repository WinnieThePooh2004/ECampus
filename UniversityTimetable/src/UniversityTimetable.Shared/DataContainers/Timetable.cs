using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
#nullable enable

namespace UniversityTimetable.Shared.DataContainers
{
    public class Timetable
    {
        /// <summary>
        /// nobody has classes on sundays, so 6
        /// </summary>
        private const int _workDaysInWeek = 6;
        private const int _maxClassesPerDay = 5;
        public AuditoryDto? Auditory { get; set; }
        public GroupDto? Group { get; set; }
        public TeacherDto? Teacher { get; set; }
        public ClassDto?[][] DailyClasses { get; set; }

        public Timetable(IEnumerable<ClassDto> classes)
        {
            DailyClasses = CreateEmptyDataTable();
            foreach (var @class in classes)
            {
                Add(@class);
            }
        }

        public Timetable()
        {
            DailyClasses = CreateEmptyDataTable();
        }

        public ClassDto? GetClass(int dayOfWeek, int number, WeekDependency weekDependency = WeekDependency.None)
        {
            if (weekDependency == WeekDependency.AppearsOnEvenWeeks)
            {
                return DailyClasses[dayOfWeek][number * 2 + 1];
            }
            return DailyClasses[dayOfWeek][number * 2];
        }
        public void Add(ClassDto @class)
        {
            if (@class.WeekDependency == WeekDependency.AppearsOnEvenWeeks)
            {
                DailyClasses[@class.DayOfWeek][@class.Number * 2 + 1] = @class;
                return;
            }
            DailyClasses[@class.DayOfWeek][@class.Number * 2] = @class;
        }

        private static ClassDto?[][] CreateEmptyDataTable()
        {
            ClassDto[][] table = new ClassDto[_workDaysInWeek][];
            for(int i = 0; i < _workDaysInWeek; i++)
            {
                table[i] = new ClassDto[_maxClassesPerDay * 2];
            }
            return table;
        }
    }
}
#nullable disable
