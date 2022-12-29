using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices;

public interface IDataUpdate<TModel>
    where TModel : class, IModel, new()
{
    Task<TModel> UpdateAsync(TModel model, DbContext context);
}