using ECampus.Shared.DataContainers;
using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Shared.Interfaces.Domain;

public interface IParametersService<TEntity, in TParams>
    where TEntity : class, IDataTransferObject
    where TParams : IQueryParameters
{
    public Task<ListWithPaginationData<TEntity>> GetByParametersAsync(TParams parameters);
}