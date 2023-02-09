using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

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