using ECampus.Domain.Commands;
using ECampus.Domain.Data;
using ECampus.Domain.Requests;
using ECampus.Domain.Responses;

namespace ECampus.Services.Contracts;

public interface IDomainAccessFacade
{
    public Task<ListWithPaginationData<TResponse>> GetByParametersAsync<TResponse, TParameter>(
        TParameter parameter, CancellationToken token)
        where TResponse : class, IMultipleItemsResponse
        where TParameter : QueryParameters<TResponse>;

    public Task<TResponse> GetByIdAsync<TResponse>(int id, CancellationToken token)
        where TResponse : class, ISingleItemResponse;
    
    public Task UpdateAsync<TUpdateCommand>(TUpdateCommand updateCommand, CancellationToken token)
        where TUpdateCommand : class, IUpdateCommand;

    public Task<CreatedResponse> CreateAsync<TCreateCommand>(TCreateCommand createCommand, CancellationToken token)
        where TCreateCommand : class, ICreateCommand;
    
    public Task DeleteAsync<TEntity>(int id, CancellationToken token)
        where TEntity : class, IEntity;
}