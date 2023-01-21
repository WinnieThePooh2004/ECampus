using ECampus.Shared.DataContainers;
using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Shared.Interfaces.DataAccess;

public interface IParametersDataAccessFacade<TEntity, in TParams>
    where TEntity : class, IModel
    where TParams : IQueryParameters<TEntity>
{
    public Task<ListWithPaginationData<TEntity>> GetByParameters(TParams parameters);
}