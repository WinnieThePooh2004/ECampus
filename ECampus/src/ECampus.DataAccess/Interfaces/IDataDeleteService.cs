using ECampus.Infrastructure;
using ECampus.Shared.Data;

namespace ECampus.DataAccess.Interfaces;

public interface IDataDeleteService<TModel>
    where TModel : class, IModel, new()
{
    Task<TModel> DeleteAsync(int id, ApplicationDbContext context, CancellationToken token = default);
}