﻿using AutoFixture;
using ECampus.Domain.Entities;

namespace ECampus.Tests.Shared.DataFactories;

public class AuditoryFactory : IAbstractFactory<Auditory>
{
    public Auditory CreateModel(Fixture fixture) =>
        fixture.Build<Auditory>()
            .Without(a => a.Classes)
            .Without(a => a.Users)
            .Without(a => a.UsersIds)
            .Create();
}