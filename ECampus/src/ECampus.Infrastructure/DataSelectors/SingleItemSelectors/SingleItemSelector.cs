using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleItemSelector<TModel> : ISingleItemSelector<TModel>
    where TModel : class, IModel, new()
{
    public async Task<TModel?> SelectModel(int id, DbSet<TModel> dataSource, CancellationToken token = default)
    {
        return await dataSource.SingleOrDefaultAsync(model => model.Id == id, token);
    }
}