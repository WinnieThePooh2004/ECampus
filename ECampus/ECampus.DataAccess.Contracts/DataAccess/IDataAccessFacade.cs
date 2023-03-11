using ECampus.Domain.Data;
using ECampus.Domain.QueryParameters;

namespace ECampus.DataAccess.Contracts.DataAccess;

public interface IDataAccessFacade
{
    TEntity Create<TEntity>(TEntity entity) where TEntity : class, IEntity;
    Task<TEntity> UpdateAsync<TEntity>(TEntity entity, CancellationToken token = default) where TEntity : class, IEntity;
    TEntity Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new();
    Task<TEntity?> GetByIdOrDefaultAsync<TEntity>(int id, CancellationToken token = default) where TEntity : class, IEntity;
    IQueryable<TModel> GetByParameters<TModel, TParameters>(TParameters parameters)
        where TModel : class, IEntity
        where TParameters : IDataSelectParameters<TModel>;

    public Task<bool> SaveChangesAsync(CancellationToken token = default);
}