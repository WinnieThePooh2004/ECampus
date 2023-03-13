using ECampus.Domain.Data;
using ECampus.Domain.Requests;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.Interfaces;

public interface IParametersSelector<out TEntity, in TParameters>
    where TEntity : class, IEntity
    where TParameters : IDataSelectParameters<TEntity>
{
    IQueryable<TEntity> SelectData(ApplicationDbContext context, TParameters parameters);
}