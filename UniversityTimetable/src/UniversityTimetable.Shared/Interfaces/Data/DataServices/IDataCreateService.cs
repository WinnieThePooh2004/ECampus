using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices;

public interface IDataCreateService<TModel>
    where TModel : class, IModel, new()
{
    Task<TModel> CreateAsync(TModel model, DbContext context);
}