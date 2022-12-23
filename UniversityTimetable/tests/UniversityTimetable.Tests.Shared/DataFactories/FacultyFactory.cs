using AutoFixture;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Shared.DataFactories;

public class FacultyFactory : IAbstractFactory<Faculty>
{
    public Faculty CreateModel(Fixture fixture) =>
        fixture.Build<Faculty>()
            .Without(f => f.Departments)
            .Create();
}