using ECampus.Core.Installers;
using ECampus.Domain.Commands;
using ECampus.Domain.Data;
using ECampus.Domain.Requests;
using ECampus.Domain.Responses;
using ECampus.Services.Contracts;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services;

[Inject(typeof(IDomainAccessFacade))]
public class DomainAccessFacade : IDomainAccessFacade
{
    private readonly IServiceProvider _serviceProvider;

    public DomainAccessFacade(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ListWithPaginationData<TResponse>>
        GetByParametersAsync<TResponse, TParameter>(TParameter parameter, CancellationToken token)
        where TResponse : class, IMultipleItemsResponse where TParameter : QueryParameters<TResponse>
    {
        var handler = _serviceProvider.GetRequiredService<IGetByParametersHandler<TResponse, TParameter>>();
        return await handler.GetByParametersAsync(parameter, token);
    }

    public async Task<TResponse> GetByIdAsync<TResponse>(int id, CancellationToken token)
        where TResponse : class, ISingleItemResponse
    {
        var handler = _serviceProvider.GetRequiredService<IGetByIdHandler<TResponse>>();
        return await handler.GetByIdAsync(id, token);
    }
    
    public async Task<CreatedResponse> CreateAsync<TCreateCommand>(TCreateCommand createCommand, CancellationToken token)
        where TCreateCommand : class, ICreateCommand
    {
        var handler = _serviceProvider.GetRequiredService<ICreateHandler<TCreateCommand>>();
        return await handler.CreateAsync(createCommand, token);
    }
    
    public async Task UpdateAsync<TUpdateCommand>(TUpdateCommand updateCommand, CancellationToken token) 
        where TUpdateCommand : class, IUpdateCommand
    {
        var handler = _serviceProvider.GetRequiredService<IUpdateHandler<TUpdateCommand>>();
        await handler.UpdateAsync(updateCommand, token);
    }


    public async Task DeleteAsync<TEntity>(int id, CancellationToken token)
        where TEntity : class, IEntity
    {
        var handler = _serviceProvider.GetRequiredService<IDeleteHandler<TEntity>>();
        await handler.DeleteAsync(id, token);
    }
}