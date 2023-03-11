using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.Interfaces;

public interface IDataUpdateService<TEntity>
    where TEntity : class, IEntity
{
    Task<TEntity> UpdateAsync(TEntity entity, ApplicationDbContext context, CancellationToken token = default);
}