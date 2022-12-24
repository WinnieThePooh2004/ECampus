using Microsoft.EntityFrameworkCore;

namespace UniversityTimetable.Shared.Interfaces.Data;

public interface IDataDelete<TModel>
    where TModel : class, IModel, new()
{
    Task DeleteAsync(int id, DbContext context);
}