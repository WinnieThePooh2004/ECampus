using AutoFixture;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Shared.DataFactories;

public class ClassFactory : IAbstractFactory<Class>
{
    public Class CreateModel(Fixture fixture) =>
        fixture.Build<Class>()
            .Without(c => c.Auditory)
            .Without(c => c.Group)
            .Without(c => c.Teacher)
            .Without(c => c.Subject)
            .Create();
}