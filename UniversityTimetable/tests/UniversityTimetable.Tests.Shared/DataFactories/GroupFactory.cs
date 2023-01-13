using AutoFixture;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Shared.DataFactories;

public class GroupFactory : IAbstractFactory<Group>
{
    public Group CreateModel(Fixture fixture) =>
        fixture.Build<Group>()
            .Without(g => g.Classes)
            .Without(g => g.Users)
            .Without(g => g.UsersIds)
            .Without(g => g.Department)
            .Without(g => g.Students)
            .Create();
}