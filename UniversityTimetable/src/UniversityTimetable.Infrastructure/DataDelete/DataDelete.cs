using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.DataDelete;

public class DataDelete<TModel> : IDataDelete<TModel>
    where TModel : class, IModel, new()
{
    public Task DeleteAsync(int id, DbContext context)
    {
        var model = new TModel { Id = id };
        context.Remove(model);
        return Task.FromResult(model);
    }
}