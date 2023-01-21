namespace ECampus.FrontEnd.Requests.Interfaces;

public interface IBaseRequests<TEntity>
{
    Task<TEntity> GetByIdAsync(int id);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
}