using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Data;

namespace ECampus.DataAccess.DataUpdateServices;

public class DataUpdateService<TModel> : IDataUpdateService<TModel>
    where TModel : class, IModel, new()
{
    public Task<TModel> UpdateAsync(TModel model, ApplicationDbContext context, CancellationToken token = default)
    {
        context.Update(model);
        return Task.FromResult(model);
    }
}