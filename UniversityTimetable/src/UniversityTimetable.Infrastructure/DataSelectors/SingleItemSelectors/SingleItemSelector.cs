using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleItemSelector<TModel> : ISingleItemSelector<TModel>
    where TModel : class, IModel
{
    public async Task<TModel> SelectModel(int id, DbSet<TModel> dataSource)
    {
        return await dataSource.FirstOrDefaultAsync(model => model.Id == id);
    }
}