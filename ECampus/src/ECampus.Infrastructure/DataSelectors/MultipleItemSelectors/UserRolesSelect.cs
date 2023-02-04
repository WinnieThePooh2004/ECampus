using ECampus.Contracts.DataSelectParameters;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class UserRolesSelect : IMultipleItemSelector<User, UserRolesParameters>
{
    public IQueryable<User> SelectData(ApplicationDbContext context, UserRolesParameters parameters)
    {
        return context.Users
            .Include(user => user.Student)
            .Include(user => user.Teacher)
            .Where(user => user.Id == parameters.UserId);
    }
}