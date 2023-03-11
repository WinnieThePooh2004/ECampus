using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataUpdateServices;

public class DataUpdateService<TEntity> : IDataUpdateService<TEntity>
    where TEntity : class, IEntity, new()
{
    public Task<TEntity> UpdateAsync(TEntity entity, ApplicationDbContext context, CancellationToken token = default)
    {
        context.Update(entity);
        return Task.FromResult(entity);
    }
}