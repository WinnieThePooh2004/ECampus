using ECampus.Shared.Data;

namespace ECampus.Contracts.Services;

public interface IBaseService<TEntity> where TEntity : class, IDataTransferObject
{
    public Task<TEntity> GetByIdAsync(int? id);
    public Task<TEntity> CreateAsync(TEntity entity);
    public Task<TEntity> UpdateAsync(TEntity entity);
    public Task<TEntity> DeleteAsync(int? id);
}