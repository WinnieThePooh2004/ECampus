using UniversityTimetable.Shared.General;
using UniversityTimetable.Shared.Interfaces.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
#nullable enable

namespace UniversityTimetable.Shared.Pagination
{
    public class Timetable<TClass> where TClass : IClass
    {
        /// <summary>
        /// nobody has classes on sundays, so 6
        /// </summary>
        public static int WorkDaysInWeek => 6;
        public static int MaxClassesPerDay => 5;
        public int AuditoryId { get; set; }
        public int GroupId { get; set; }
        public int TeacherId { get; set; }
        public TClass?[,] DailyClasses { get; set; }

        public Timetable(TClass?[,] dailyClasses, int auditoryId, int groupId, int teacherId)
        {
            AuditoryId = auditoryId;
            GroupId = groupId;
            TeacherId = teacherId;
            DailyClasses = dailyClasses;
        }

        public Timetable()
            : this(CreateEmptyDataTable(), 0, 0, 0)
        {

        }

        public TClass? GetClass(int dayOfWeek, int number, WeekDependency weekDependency = WeekDependency.None)
        {
            if (weekDependency == WeekDependency.AppearsOnEvenWeeks)
            {
                return DailyClasses[dayOfWeek, number * 2 + 1];
            }
            return DailyClasses[dayOfWeek, number * 2];
        }
        public void Add(TClass @class)
        {
            if (@class.WeekDependency == WeekDependency.AppearsOnEvenWeeks)
            {
                DailyClasses[@class.DayOfTheWeek, @class.Number * 2 + 1] = @class;
                return;
            }
            DailyClasses[@class.DayOfTheWeek, @class.Number * 2] = @class;
        }

        private static TClass?[,] CreateEmptyDataTable()
            => new TClass[WorkDaysInWeek, MaxClassesPerDay * 2];
    }
}
#nullable disable
