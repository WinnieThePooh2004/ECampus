using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataCreateServices;

public class DataCreateService<TEntity> : IDataCreateService<TEntity> 
    where TEntity : class, IEntity, new()
{
    public TEntity Create(TEntity model, ApplicationDbContext context)
    {
        context.Add(model);
        return model;
    }
}