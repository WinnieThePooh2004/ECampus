using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Shared.Interfaces.Data.DataServices;

public interface ISingleItemSelector<TModel>
    where TModel : class, IModel
{
    Task<TModel?> SelectModel(int id, DbSet<TModel> dataSource);
}