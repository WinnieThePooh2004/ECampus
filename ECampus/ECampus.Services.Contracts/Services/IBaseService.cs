using ECampus.Domain.Data;

namespace ECampus.Services.Contracts.Services;

public interface IBaseService<TDto> where TDto : class, IDataTransferObject
{
    public Task<TDto> GetByIdAsync(int id, CancellationToken token = default);
    public Task<TDto> CreateAsync(TDto dto, CancellationToken token = default);
    public Task<TDto> UpdateAsync(TDto dto, CancellationToken token = default);
    public Task<TDto> DeleteAsync(int id, CancellationToken token = default);
}