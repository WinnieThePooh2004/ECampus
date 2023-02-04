using ECampus.Contracts.DataSelectParameters;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class UserEmailSelector : IMultipleItemSelector<User, UserEmailParameters>
{
    public IQueryable<User> SelectData(ApplicationDbContext context, UserEmailParameters parameters)
    {
        return context.Users.Where(user => user.Email == parameters.Email);
    }
}