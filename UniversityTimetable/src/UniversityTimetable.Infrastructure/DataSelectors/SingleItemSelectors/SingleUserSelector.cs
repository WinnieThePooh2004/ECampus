using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleUserSelector : ISingleItemSelector<User>
{
    public async Task<User?> SelectModel(int id, DbSet<User> dataSource)
        => await dataSource.Include(u => u.SavedAuditories)
            .Include(u => u.SavedGroups)
            .Include(u => u.SavedTeachers)
            .FirstOrDefaultAsync(u => u.Id == id);
}