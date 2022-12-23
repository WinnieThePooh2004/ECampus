using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleTeacherSelector : ISingleItemSelector<Teacher>
{
    public async Task<Teacher> SelectModel(int id, DbSet<Teacher> dataSource)
        => await dataSource.Include(t => t.Subjects)
            .FirstOrDefaultAsync(t => t.Id == id);
}