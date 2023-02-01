using ECampus.Shared.Data;
using ECampus.Shared.DataContainers;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataAccess;

public interface IParametersDataAccessFacade<TEntity, in TParams>
    where TEntity : class, IModel
    where TParams : IQueryParameters<TEntity>
{
    public Task<ListWithPaginationData<TEntity>> GetByParameters(TParams parameters);
}