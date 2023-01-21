using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleCourseSelector : ISingleItemSelector<Course>
{
    public async Task<Course?> SelectModel(int id, DbSet<Course> dataSource) =>
        await dataSource
            .Include(c => c.Groups)
            .Include(c => c.Teachers)
            .FirstOrDefaultAsync(c => c.Id == id);
}