using ECampus.Shared.Data;

namespace ECampus.Infrastructure.Interfaces;

public interface IDataCreateService<TModel>
    where TModel : class, IModel
{
    Task<TModel> CreateAsync(TModel model, ApplicationDbContext context, CancellationToken token = default);
}