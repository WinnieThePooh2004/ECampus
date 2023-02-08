using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Data;

namespace ECampus.Infrastructure.DataCreateServices;

public class DataCreateService<TModel> : IDataCreateService<TModel> 
    where TModel : class, IModel, new()
{
    public async Task<TModel> CreateAsync(TModel model, ApplicationDbContext context, CancellationToken token = default)
    {
        await context.AddAsync(model, token);
        return model;
    }
}