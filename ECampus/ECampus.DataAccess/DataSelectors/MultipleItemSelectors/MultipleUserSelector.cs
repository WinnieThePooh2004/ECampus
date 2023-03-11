using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Extensions;
using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleUserSelector : IParametersSelector<User, UserParameters>
{
    public IQueryable<User> SelectData(ApplicationDbContext context, UserParameters parameters) =>
        context.Users.Search(u => u.Email, parameters.Email)
            .Search(u => u.Username, parameters.Username)
            .Sort(parameters.OrderBy, parameters.SortOrder);
}