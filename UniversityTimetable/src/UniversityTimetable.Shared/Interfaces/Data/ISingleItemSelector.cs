using Microsoft.EntityFrameworkCore;

namespace UniversityTimetable.Shared.Interfaces.Data;

public interface ISingleItemSelector<TModel>
    where TModel : class, IModel
{
    Task<TModel> SelectModel(int id, DbSet<TModel> dataSource);
}