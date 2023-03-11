using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Data;
using ECampus.Infrastructure;

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