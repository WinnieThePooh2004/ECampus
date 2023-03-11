using ECampus.Domain.DataContainers;
using ECampus.Domain.QueryParameters;

namespace ECampus.FrontEnd.Requests.Interfaces;

public interface IParametersRequests<TEntity, in TParams> 
    where TParams : IQueryParameters
{
    public Task<ListWithPaginationData<TEntity>> GetByParametersAsync(TParams parameters);
}