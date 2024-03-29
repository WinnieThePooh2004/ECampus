﻿using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Entities;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class UserUsernameSelector : IParametersSelector<User, UserUsernameParameters>
{
    public IQueryable<User> SelectData(ApplicationDbContext context, UserUsernameParameters parameters) =>
        context.Users.Where(user => user.Username == parameters.Username);
}