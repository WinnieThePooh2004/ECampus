using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataUpdateServices;

public class DataUpdateService<TModel> : IDataUpdateService<TModel>
    where TModel : class, IModel, new()
{
    public Task<TModel> UpdateAsync(TModel model, DbContext context)
    {
        context.Update(model);
        return Task.FromResult(model);
    }
}