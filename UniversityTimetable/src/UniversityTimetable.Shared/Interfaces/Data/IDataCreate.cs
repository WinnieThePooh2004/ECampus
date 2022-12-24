using Microsoft.EntityFrameworkCore;

namespace UniversityTimetable.Shared.Interfaces.Data;

public interface IDataCreate<TModel>
    where TModel : class, IModel, new()
{
    Task<TModel> CreateAsync(TModel model, DbContext context);
}