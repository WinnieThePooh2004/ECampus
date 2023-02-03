using ECampus.Shared.Data;
using ECampus.Shared.DataContainers;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataAccess;

public interface IParametersDataAccessFacade<out TEntity, in TParams>
    where TEntity : class, IModel
    where TParams : IQueryParameters<TEntity>
{
    public IQueryable<TEntity> GetByParameters(TParams parameters);
}