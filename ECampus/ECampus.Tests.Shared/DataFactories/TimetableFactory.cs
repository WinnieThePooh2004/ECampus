using AutoFixture;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Responses.Class;

namespace ECampus.Tests.Shared.DataFactories;

public class TimetableFactory : IAbstractFactory<Timetable>
{ public Timetable CreateModel(Fixture fixture)
    {
        var rand = new Random();
        return new Timetable(Enumerable.Range(0, 10)
            .Select(_ => fixture.Build<ClassDto>()
                .With(c => c.Number, rand.Next(0, 5))
                .With(c => c.DayOfWeek, rand.Next(0, 6))
                .Create()).ToList());
    }
}