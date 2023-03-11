using ECampus.Domain.Data;
using ECampus.Domain.DataContainers;
using ECampus.Domain.QueryParameters;

namespace ECampus.Services.Contracts.Services;

public interface IParametersService<TDto, in TParams>
    where TDto : class, IDataTransferObject
    where TParams : IQueryParameters<TDto>
{
    public Task<ListWithPaginationData<TDto>> GetByParametersAsync(TParams parameters, CancellationToken token = default);
}