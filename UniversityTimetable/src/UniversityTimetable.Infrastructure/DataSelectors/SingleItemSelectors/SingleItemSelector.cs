using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleItemSelector<TModel> : ISingleItemSelector<TModel>
    where TModel : class, IModel, new()
{
    public async Task<TModel?> SelectModel(int id, DbSet<TModel> dataSource)
    {
        return await dataSource.FindAsync(id);
    }
}