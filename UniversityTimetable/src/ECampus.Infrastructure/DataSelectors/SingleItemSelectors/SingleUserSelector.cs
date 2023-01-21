using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleUserSelector : ISingleItemSelector<User>
{
    public async Task<User?> SelectModel(int id, DbSet<User> dataSource)
        => await dataSource.Include(u => u.SavedAuditories)
            .Include(u => u.SavedGroups)
            .Include(u => u.SavedTeachers)
            .FirstOrDefaultAsync(u => u.Id == id);
}