using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Data;

namespace ECampus.Infrastructure.DataUpdateServices;

public class DataUpdateService<TModel> : IDataUpdateService<TModel>
    where TModel : class, IModel, new()
{
    public Task<TModel> UpdateAsync(TModel model, ApplicationDbContext context)
    {
        context.Update(model);
        return Task.FromResult(model);
    }
}