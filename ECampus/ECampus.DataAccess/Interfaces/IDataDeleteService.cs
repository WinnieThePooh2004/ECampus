using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.Interfaces;

public interface IDataDeleteService<TEntity>
    where TEntity : class, IEntity
{
    TEntity Delete(TEntity model, ApplicationDbContext context);
}