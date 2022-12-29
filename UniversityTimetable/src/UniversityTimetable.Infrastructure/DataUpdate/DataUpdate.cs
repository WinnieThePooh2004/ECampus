using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.DataUpdate;

public class DataUpdate<TModel> : IDataUpdate<TModel>
    where TModel : class, IModel, new()
{
    public Task<TModel> UpdateAsync(TModel model, DbContext context)
    {
        context.Update(model);
        return Task.FromResult(model);
    }
}