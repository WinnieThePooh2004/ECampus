using ECampus.Contracts.DataSelectParameters;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class UserUsernameSelector : IMultipleItemSelector<User, UserUsernameParameters>
{
    public IQueryable<User> SelectData(ApplicationDbContext context, UserUsernameParameters parameters) =>
        context.Users.Where(user => user.Username == parameters.Username);
}