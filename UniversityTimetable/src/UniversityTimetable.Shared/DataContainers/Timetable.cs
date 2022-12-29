using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;

namespace UniversityTimetable.Shared.DataContainers;

public class Timetable
{
    /// <summary>
    /// nobody has classes on sundays, so 6
    /// </summary>
    private const int WorkDaysInWeek = 6;

    private const int MaxClassesPerDay = 5;
    public AuditoryDto? Auditory { get; set; }
    public GroupDto? Group { get; set; }
    public TeacherDto? Teacher { get; set; }
    public ClassDto?[][] DailyClasses { get; set; }

    public Timetable(List<ClassDto> classes)
    {
        DailyClasses = CreateEmptyDataTable();
        classes.ForEach(Add);
    }
    
    public ClassDto? GetClass(int dayOfWeek, int number, WeekDependency weekDependency = WeekDependency.None)
    {
        return weekDependency == WeekDependency.AppearsOnEvenWeeks
            ? DailyClasses[dayOfWeek][number * 2 + 1]
            : DailyClasses[dayOfWeek][number * 2];
    }

    private void Add(ClassDto @class)
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
        var table = new ClassDto[WorkDaysInWeek][];
        for (var i = 0; i < WorkDaysInWeek; i++)
        {
            table[i] = new ClassDto[MaxClassesPerDay * 2];
        }

        return table;
    }
}