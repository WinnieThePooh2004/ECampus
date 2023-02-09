using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Data;

namespace ECampus.DataAccess.DataDeleteServices;

public class DataDeleteService<TModel> : IDataDeleteService<TModel>
    where TModel : class, IModel, new()
{
    public Task<TModel> DeleteAsync(int id, ApplicationDbContext context, CancellationToken token = default)
    {
        var model = new TModel { Id = id };
        context.Remove(model);
        return Task.FromResult(model);
    }
}