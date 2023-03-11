using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.SingleItemSelectors;

public class SingleTeacherRateSelect : ISingleItemSelector<TeacherRate>
{
    public async Task<TeacherRate?> SelectModel(int id, DbSet<TeacherRate> dataSource, CancellationToken token = default) =>
        await dataSource.Include(c => c.Teacher)
            .Include(c => c.Course)
            .SingleOrDefaultAsync(t => t.Id == id, token);
}