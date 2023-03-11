using ECampus.Domain.Data;
using ECampus.Domain.DataContainers;
using ECampus.Domain.QueryParameters;

namespace ECampus.Services.Contracts.Services;

public interface IParametersService<TEntity, in TParams>
    where TEntity : class, IDataTransferObject
    where TParams : IQueryParameters<TEntity>
{
    public Task<ListWithPaginationData<TEntity>> GetByParametersAsync(TParams parameters, CancellationToken token = default);
}