using AutoFixture;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Shared.DataFactories;

public class TeacherFactory : IAbstractFactory<Teacher>
{
    public Teacher CreateModel(Fixture fixture) =>
        fixture.Build<Teacher>()
            .Without(t => t.Classes)
            .Without(t => t.Department)
            .Without(t => t.Users)
            .Without(t => t.UsersIds)
            .Without(t => t.Subjects)
            .Without(t => t.SubjectIds)
            .Create();
}