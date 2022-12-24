using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Infrastructure.DataCreate;

public class DataCreate<TModel> : IDataCreate<TModel> 
    where TModel : class, IModel, new()
{
    public async Task<TModel> CreateAsync(TModel model, DbContext context)
    {
        await context.AddAsync(model);
        return model;
    }
}