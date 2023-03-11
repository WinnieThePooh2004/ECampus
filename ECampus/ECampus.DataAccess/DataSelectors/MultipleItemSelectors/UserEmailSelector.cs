using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Entities;
using ECampus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class UserEmailSelector : IParametersSelector<User, UserEmailParameters>
{
    public IQueryable<User> SelectData(ApplicationDbContext context, UserEmailParameters parameters)
    {
        return context.Users
            .Include(u => u.Student)
            .Where(user => user.Email == parameters.Email);
    }
}