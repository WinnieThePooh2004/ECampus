using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices;

// ReSharper disable once UnusedTypeParameter
public interface IDataDeleteService<TModel>
    where TModel : class, IModel, new()
{
    Task<TModel> DeleteAsync(int id, DbContext context);
}