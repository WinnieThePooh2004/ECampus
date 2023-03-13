using ECampus.Domain.Requests;
using ECampus.Domain.Responses;

namespace ECampus.Services.Contracts.Services;

public interface IParametersService<TResponse, in TParams>
    where TResponse : class, IMultipleItemsResponse
    where TParams : IQueryParameters<TResponse>
{
    public Task<ListWithPaginationData<TResponse>> GetByParametersAsync(TParams parameters, CancellationToken token = default);
}