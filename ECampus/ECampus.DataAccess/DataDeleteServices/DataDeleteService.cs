using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataDeleteServices;

public class DataDeleteService<TEntity> : IDataDeleteService<TEntity>
    where TEntity : class, IEntity, new()
{
    public TEntity Delete(TEntity model, ApplicationDbContext context)
    {
        context.Remove(model);
        return model;
    }
}