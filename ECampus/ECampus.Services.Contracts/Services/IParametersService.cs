using ECampus.Shared.Data;
using ECampus.Shared.DataContainers;
using ECampus.Shared.QueryParameters;

namespace ECampus.Services.Contracts.Services;

public interface IParametersService<TEntity, in TParams>
    where TEntity : class, IDataTransferObject
    where TParams : IQueryParameters<TEntity>
{
    public Task<ListWithPaginationData<TEntity>> GetByParametersAsync(TParams parameters, CancellationToken token = default);
}