using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain;

public class TimetableTests
{
    [Fact]
    private void CreateTimetable_WidthShouldBe6_HeightShouldBe5()
    {
        var timetable = new Timetable(new List<ClassDto>());
        timetable.DailyClasses.Length.Should().Be(6);
        timetable.DailyClasses[0].Length.Should().Be(10);
    }

    [Fact]
    private void AddClass_WeekDependencyNone_ShouldBeOnRightPlace()
    {
        var @class = new ClassDto { Number = 3, DayOfWeek = 4, WeekDependency = WeekDependency.None };
        var timetable = new Timetable(new List<ClassDto>{ @class });

        timetable.DailyClasses[4][6].Should().Be(@class);
    }
    
    [Fact]
    private void AddClass_WeekDependencyOdd_ShouldBeOnRightPlace()
    {
        var @class = new ClassDto { Number = 3, DayOfWeek = 4, WeekDependency = WeekDependency.AppearsOnOddWeeks };
        var timetable = new Timetable(new List<ClassDto>{ @class });

        timetable.DailyClasses[4][6].Should().Be(@class);
    }

    [Fact]
    private void AddClass_WeekDependencyEven_ShouldBeOnRightPlace()
    {
        var @class = new ClassDto { Number = 3, DayOfWeek = 4, WeekDependency = WeekDependency.AppearsOnEvenWeeks };
        var timetable = new Timetable(new List<ClassDto>{ @class });

        timetable.DailyClasses[4][7].Should().Be(@class);
    }

    [Fact]
    private void GetClass_Day5Number2WeekDependencyNone_ShouldReturnClassFromItsPlace()
    {
        var @class = new ClassDto { DayOfWeek = 5, Number = 2, WeekDependency = WeekDependency.None };
        var timetable = new Timetable(new List<ClassDto>{ @class });

        timetable.GetClass(5, 2).Should().Be(@class);
    }
    
    [Fact]
    private void GetClass_Day5Number2WeekDependencyOdd_ShouldReturnClassFromItsPlace()
    {
        var @class = new ClassDto { DayOfWeek = 5, Number = 2, WeekDependency = WeekDependency.AppearsOnOddWeeks };
        var timetable = new Timetable(new List<ClassDto>{ @class });

        timetable.GetClass(5, 2, WeekDependency.AppearsOnOddWeeks).Should().Be(@class);
    }

    [Fact]
    private void GetClass_Day5Number2WeekDependencyEven_ShouldReturnClassFromItsPlace()
    {
        var @class = new ClassDto { DayOfWeek = 5, Number = 2, WeekDependency = WeekDependency.AppearsOnEvenWeeks };
        var timetable = new Timetable(new List<ClassDto>{ @class });

        timetable.GetClass(5, 2, WeekDependency.AppearsOnEvenWeeks).Should().Be(@class);
    }

    [Fact]
    private void GetClass_ShouldReturnNullIfClassDoesNotExists()
    {
        new Timetable(new List<ClassDto>()).GetClass(0, 0).Should().BeNull();
    }
}