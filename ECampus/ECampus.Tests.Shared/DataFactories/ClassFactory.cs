using AutoFixture;
using ECampus.Domain.Models;

namespace ECampus.Tests.Shared.DataFactories;

public class ClassFactory : IAbstractFactory<Class>
{
    public Class CreateModel(Fixture fixture)
    {
        var rand = new Random();
        return fixture.Build<Class>()
            .Without(c => c.Auditory)
            .Without(c => c.Group)
            .Without(c => c.Teacher)
            .Without(c => c.Subject)
            .With(c => c.Number, rand.Next(0, 5))
            .With(c => c.DayOfWeek, rand.Next(0, 6))
            .Create();
    }
}