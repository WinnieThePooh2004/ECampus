using ECampus.Infrastructure;
using ECampus.Shared.Data;

namespace ECampus.DataAccess.Interfaces;

public interface IDataCreateService<TModel>
    where TModel : class, IModel
{
    Task<TModel> CreateAsync(TModel model, ApplicationDbContext context, CancellationToken token = default);
}