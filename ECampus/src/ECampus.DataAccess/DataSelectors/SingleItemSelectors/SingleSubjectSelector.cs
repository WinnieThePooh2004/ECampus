using ECampus.DataAccess.Interfaces;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.SingleItemSelectors;

public class SingleSubjectSelector : ISingleItemSelector<Subject>
{
    public async Task<Subject?> SelectModel(int id, DbSet<Subject> dataSource, CancellationToken token = default)
        => await dataSource.Include(t => t.Teachers)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken: token);
}