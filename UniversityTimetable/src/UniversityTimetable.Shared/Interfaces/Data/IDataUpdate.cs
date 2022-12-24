using Microsoft.EntityFrameworkCore;

namespace UniversityTimetable.Shared.Interfaces.Data;

public interface IDataUpdate<TModel>
    where TModel : class, IModel, new()
{
    Task<TModel> UpdateAsync(TModel model, DbContext context);
}