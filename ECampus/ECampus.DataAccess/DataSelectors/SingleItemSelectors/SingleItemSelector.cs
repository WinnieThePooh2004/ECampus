using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.SingleItemSelectors;

public class SingleItemSelector<TEntity> : ISingleItemSelector<TEntity>
    where TEntity : class, IEntity, new()
{
    public async Task<TEntity?> SelectModel(int id, DbSet<TEntity> dataSource, CancellationToken token = default)
    {
        return await dataSource.SingleOrDefaultAsync(model => model.Id == id, token);
    }
}