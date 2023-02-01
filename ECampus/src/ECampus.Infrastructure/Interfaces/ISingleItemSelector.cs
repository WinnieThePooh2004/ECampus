using ECampus.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.Interfaces;

public interface ISingleItemSelector<TModel>
    where TModel : class, IModel
{
    Task<TModel?> SelectModel(int id, DbSet<TModel> dataSource);
}