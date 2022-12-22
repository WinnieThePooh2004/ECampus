using AutoFixture;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Shared.DataFactories;

public class DepartmentFactory : IAbstractFactory<Department>
{
    public Department CreateModel(Fixture fixture) =>
        fixture.Build<Department>()
            .Without(d => d.Groups)
            .Without(d => d.Faculty)
            .Without(d => d.Teachers)
            .Create();
}