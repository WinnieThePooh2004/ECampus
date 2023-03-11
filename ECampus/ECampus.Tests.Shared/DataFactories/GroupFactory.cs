using AutoFixture;
using ECampus.Domain.Entities;

namespace ECampus.Tests.Shared.DataFactories;

public class GroupFactory : IAbstractFactory<Group>
{
    public Group CreateModel(Fixture fixture) =>
        fixture.Build<Group>()
            .Without(g => g.Classes)
            .Without(g => g.Users)
            .Without(g => g.UsersIds)
            .Without(g => g.Department)
            .Without(g => g.Students)
            .Without(g => g.Courses)
            .Without(g => g.CourseGroups)
            .Create();
}