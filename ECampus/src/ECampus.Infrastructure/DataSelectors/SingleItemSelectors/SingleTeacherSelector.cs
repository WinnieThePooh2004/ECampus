using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleTeacherSelector : ISingleItemSelector<Teacher>
{
    public async Task<Teacher?> SelectModel(int id, DbSet<Teacher> dataSource)
        => await dataSource.Include(t => t.Subjects)
            .FirstOrDefaultAsync(t => t.Id == id);
}