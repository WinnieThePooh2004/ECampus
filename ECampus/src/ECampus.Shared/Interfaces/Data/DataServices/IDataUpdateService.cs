using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Shared.Interfaces.Data.DataServices;

public interface IDataUpdateService<TModel>
    where TModel : class, IModel
{
    Task<TModel> UpdateAsync(TModel model, DbContext context);
}