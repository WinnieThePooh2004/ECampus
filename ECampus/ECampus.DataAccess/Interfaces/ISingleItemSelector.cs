using ECampus.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.Interfaces;

public interface ISingleItemSelector<TEntity>
    where TEntity : class, IEntity
{
    Task<TEntity?> SelectModel(int id, DbSet<TEntity> dataSource, CancellationToken token = default);
}