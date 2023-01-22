﻿using ECampus.Shared.Extensions;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleUserSelector : IMultipleItemSelector<User, UserParameters>
{
    public IQueryable<User> SelectData(DbSet<User> data, UserParameters parameters) =>
        data.Search(u => u.Email, parameters.Email)
            .Search(u => u.Username, parameters.Username)
            .Where(u => u.Role >= parameters.Role)
            .Sort(parameters.OrderBy, parameters.SortOrder);
}