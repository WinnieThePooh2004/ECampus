using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleUserSelector : IMultipleItemSelector<User, UserParameters>
{
    public IQueryable<User> SelectData(ApplicationDbContext context, UserParameters parameters) =>
        context.Users.Search(u => u.Email, parameters.Email)
            .Search(u => u.Username, parameters.Username)
            .Sort(parameters.OrderBy, parameters.SortOrder);
}