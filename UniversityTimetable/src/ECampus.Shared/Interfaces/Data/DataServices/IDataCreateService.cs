using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Shared.Interfaces.Data.DataServices;

public interface IDataCreateService<TModel>
    where TModel : class, IModel
{
    Task<TModel> CreateAsync(TModel model, DbContext context);
}