using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices;

public interface IDataUpdateService<TModel>
    where TModel : class, IModel, new()
{
    Task<TModel> UpdateAsync(TModel model, DbContext context);
}