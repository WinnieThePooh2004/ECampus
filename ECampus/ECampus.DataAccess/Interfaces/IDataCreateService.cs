using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.Interfaces;

public interface IDataCreateService<TEntity>
    where TEntity : class, IEntity
{
    TEntity Create(TEntity model, ApplicationDbContext context);
}