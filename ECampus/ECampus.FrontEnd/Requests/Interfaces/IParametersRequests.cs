using ECampus.Domain.Requests;
using ECampus.Domain.Responses;

namespace ECampus.FrontEnd.Requests.Interfaces;

public interface IParametersRequests<TEntity, in TParams> 
    where TParams : IQueryParameters 
    where TEntity : IMultipleItemsResponse
{
    public Task<ListWithPaginationData<TEntity>> GetByParametersAsync(TParams parameters);
}