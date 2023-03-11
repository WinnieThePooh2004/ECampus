using ECampus.DataAccess.Interfaces;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.SingleItemSelectors;

public class SingleTeacherSelector : ISingleItemSelector<Teacher>
{
    public async Task<Teacher?> SelectModel(int id, DbSet<Teacher> dataSource, CancellationToken token = default)
        => await dataSource.Include(t => t.Subjects)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken: token);
}