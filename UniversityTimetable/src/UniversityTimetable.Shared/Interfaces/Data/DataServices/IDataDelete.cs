using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices;

// ReSharper disable once UnusedTypeParameter
public interface IDataDelete<TModel>
    where TModel : class, IModel, new()
{
    Task DeleteAsync(int id, DbContext context);
}