using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.DataUpdateServices;

public class DataUpdateService<TModel> : IDataUpdateService<TModel>
    where TModel : class, IModel, new()
{
    public Task<TModel> UpdateAsync(TModel model, DbContext context)
    {
        context.Update(model);
        return Task.FromResult(model);
    }
}