using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleItemSelector<TModel> : ISingleItemSelector<TModel>
    where TModel : class, IModel, new()
{
    public async Task<TModel?> SelectModel(int id, DbSet<TModel> dataSource)
    {
        return await dataSource.FindAsync(id);
    }
}