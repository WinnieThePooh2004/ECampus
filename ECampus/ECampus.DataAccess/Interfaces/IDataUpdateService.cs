using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.Interfaces;

public interface IDataUpdateService<TModel>
    where TModel : class, IModel
{
    Task<TModel> UpdateAsync(TModel model, ApplicationDbContext context, CancellationToken token = default);
}