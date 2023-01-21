using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataCreateServices;

public class DataCreateService<TModel> : IDataCreateService<TModel> 
    where TModel : class, IModel, new()
{
    public async Task<TModel> CreateAsync(TModel model, DbContext context)
    {
        await context.AddAsync(model);
        return model;
    }
}