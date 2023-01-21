using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleCourseSelector : ISingleItemSelector<Course>
{
    public async Task<Course?> SelectModel(int id, DbSet<Course> dataSource) =>
        await dataSource
            .Include(c => c.Groups)
            .Include(c => c.Teachers)
            .FirstOrDefaultAsync(c => c.Id == id);
}