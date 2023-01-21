using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Shared.Interfaces.Data.DataServices;

// ReSharper disable once UnusedTypeParameter
public interface IDataDeleteService<TModel>
    where TModel : class, IModel, new()
{
    Task<TModel> DeleteAsync(int id, DbContext context);
}