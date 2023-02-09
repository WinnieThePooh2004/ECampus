using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class UserEmailSelector : IMultipleItemSelector<User, UserEmailParameters>
{
    public IQueryable<User> SelectData(ApplicationDbContext context, UserEmailParameters parameters)
    {
        return context.Users.Where(user => user.Email == parameters.Email);
    }
}