using AutoFixture;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Shared.DataFactories;

public class SubjectFactory : IAbstractFactory<Subject>
{
    public Subject CreateModel(Fixture fixture) =>
        fixture.Build<Subject>()
            .Without(s => s.Classes)
            .Without(s => s.Teachers)
            .Without(s => s.TeacherIds)
            .Create();
}