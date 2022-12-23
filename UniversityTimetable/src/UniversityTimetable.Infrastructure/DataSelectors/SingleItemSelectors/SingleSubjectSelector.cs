using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleSubjectSelector : ISingleItemSelector<Subject>
{
    public async Task<Subject> SelectModel(int id, DbSet<Subject> dataSource)
        => await dataSource.Include(t => t.Teachers)
            .FirstOrDefaultAsync(s => s.Id == id);
}