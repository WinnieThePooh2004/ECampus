using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataCreateServices;

public class DataCreateService<TModel> : IDataCreateService<TModel> 
    where TModel : class, IModel, new()
{
    public async Task<TModel> CreateAsync(TModel model, ApplicationDbContext context)
    {
        await context.AddAsync(model);
        return model;
    }
}