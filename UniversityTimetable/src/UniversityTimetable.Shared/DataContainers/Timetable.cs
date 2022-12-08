using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.General;
#nullable enable

namespace UniversityTimetable.Shared.DataContainers
{
    public class Timetable
    {
        /// <summary>
        /// nobody has classes on sundays, so 6
        /// </summary>
        public static int WorkDaysInWeek => 6;
        public static int MaxClassesPerDay => 5;
        public AuditoryDTO? Auditory { get; set; }
        public GroupDTO? Group { get; set; }
        public TeacherDTO? Teacher { get; set; }
        public ClassDTO?[][] DailyClasses { get; set; }

        public Timetable(IEnumerable<ClassDTO> classes)
        {
            DailyClasses = CreateEmptyDataTable();
            foreach (var @class in classes)
            {
                Add(@class);
            }
        }

        public ClassDTO? GeClassDTO(int dayOfWeek, int number, WeekDependency weekDependency = WeekDependency.None)
        {
            if (weekDependency == WeekDependency.AppearsOnEvenWeeks)
            {
                return DailyClasses[dayOfWeek][number * 2 + 1];
            }
            return DailyClasses[dayOfWeek][number * 2];
        }
        public void Add(ClassDTO @class)
        {
            if (@class.WeekDependency == WeekDependency.AppearsOnEvenWeeks)
            {
                DailyClasses[@class.DayOfWeek][@class.Number * 2 + 1] = @class;
                return;
            }
            DailyClasses[@class.DayOfWeek][@class.Number * 2] = @class;
        }

        private static ClassDTO?[][] CreateEmptyDataTable()
        {
            ClassDTO[][] table = new ClassDTO[WorkDaysInWeek][];
            for(int i = 0; i < WorkDaysInWeek; i++)
            {
                table[i] = new ClassDTO[MaxClassesPerDay * 2];
            }
            return table;
        }
    }
}
#nullable disable
