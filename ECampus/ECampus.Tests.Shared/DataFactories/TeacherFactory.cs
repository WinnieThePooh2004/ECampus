using AutoFixture;
using ECampus.Domain.Models;

namespace ECampus.Tests.Shared.DataFactories;

public class TeacherFactory : IAbstractFactory<Teacher>
{
    public Teacher CreateModel(Fixture fixture) =>
        fixture.Build<Teacher>()
            .Without(t => t.Classes)
            .Without(t => t.Department)
            .Without(t => t.Users)
            .Without(t => t.UsersIds)
            .Without(t => t.Subjects)
            .Without(t => t.User)
            .Without(t => t.SubjectIds)
            .Without(t => t.Courses)
            .Without(t => t.CourseTeachers)
            .Without(t => t.Rates)
            .Create();
}