using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleUserSelector : IParametersSelector<User, UserParameters>
{
    public IQueryable<User> SelectData(ApplicationDbContext context, UserParameters parameters) =>
        context.Users.Search(u => u.Email, parameters.Email)
            .Search(u => u.Username, parameters.Username)
            .Sort(parameters.OrderBy, parameters.SortOrder);
}