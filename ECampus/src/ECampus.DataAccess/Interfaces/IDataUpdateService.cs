using ECampus.Infrastructure;
using ECampus.Shared.Data;

namespace ECampus.DataAccess.Interfaces;

public interface IDataUpdateService<TModel>
    where TModel : class, IModel
{
    Task<TModel> UpdateAsync(TModel model, ApplicationDbContext context, CancellationToken token = default);
}