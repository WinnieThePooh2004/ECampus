using ECampus.Shared.Data;

namespace ECampus.Services.Contracts.Services;

public interface IBaseService<TEntity> where TEntity : class, IDataTransferObject
{
    public Task<TEntity> GetByIdAsync(int id, CancellationToken token = default);
    public Task<TEntity> CreateAsync(TEntity entity, CancellationToken token = default);
    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken token = default);
    public Task<TEntity> DeleteAsync(int id, CancellationToken token = default);
}