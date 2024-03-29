﻿using AutoFixture;
using ECampus.Domain.Entities;

namespace ECampus.Tests.Shared.DataFactories;

public class UserFactory : IAbstractFactory<User>
{
    public User CreateModel(Fixture fixture) =>
        fixture.Build<User>()
            .Without(u => u.SavedAuditories)
            .Without(u => u.SavedAuditoriesIds)
            .Without(u => u.SavedGroups)
            .Without(u => u.SavedGroupsIds)
            .Without(u => u.SavedTeachers)
            .Without(u => u.Student)
            .Without(u => u.Teacher)
            .Without(u => u.SavedTeachersIds)
            .Create();
}