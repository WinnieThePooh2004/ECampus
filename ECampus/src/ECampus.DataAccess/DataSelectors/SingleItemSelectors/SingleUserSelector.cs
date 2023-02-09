using ECampus.DataAccess.Interfaces;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.SingleItemSelectors;

public class SingleUserSelector : ISingleItemSelector<User>
{
    public async Task<User?> SelectModel(int id, DbSet<User> dataSource, CancellationToken token = default)
        => await dataSource.Include(u => u.SavedAuditories)
            .Include(u => u.SavedGroups)
            .Include(u => u.SavedTeachers)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: token);
}