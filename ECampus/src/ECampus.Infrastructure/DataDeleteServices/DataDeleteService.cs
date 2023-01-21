using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataDeleteServices;

public class DataDeleteService<TModel> : IDataDeleteService<TModel>
    where TModel : class, IModel, new()
{
    public Task<TModel> DeleteAsync(int id, DbContext context)
    {
        var model = new TModel { Id = id };
        context.Remove(model);
        return Task.FromResult(model);
    }
}