using ECampus.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.SingleItemSelectors;

public static class DbSetExtensions
{
    public static async Task<TModel?> GetPureByIdAsync<TModel>(this DbSet<TModel> dataSource, int id, CancellationToken token = default) 
        where TModel : class, IModel
    {
        return await dataSource.SingleOrDefaultAsync(model => model.Id == id, token);
    }
}