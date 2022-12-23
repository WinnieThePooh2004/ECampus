using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleClassSelector : ISingleItemSelector<Class>
{
    public Task<Class> SelectModel(int id, DbSet<Class> dataSource)
        => dataSource.FirstOrDefaultAsync(c => c.Id == id);
}