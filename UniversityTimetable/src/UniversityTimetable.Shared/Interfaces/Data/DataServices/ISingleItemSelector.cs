using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices;

public interface ISingleItemSelector<TModel>
    where TModel : class, IModel
{
    Task<TModel?> SelectModel(int id, DbSet<TModel> dataSource);
}