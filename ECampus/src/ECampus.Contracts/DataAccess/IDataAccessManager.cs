using ECampus.Shared.Data;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataAccess;

public interface IDataAccessManager
{
    TModel CreateAsync<TModel>(TModel model) where TModel : class, IModel;
    Task<TModel> UpdateAsync<TModel>(TModel model, CancellationToken token = default) where TModel : class, IModel;
    TModel Delete<TModel>(TModel model) where TModel : class, IModel, new();
    Task<TModel> GetByIdAsync<TModel>(int id, CancellationToken token = default) where TModel : class, IModel;
    IQueryable<TModel> GetByParameters<TModel, TParameters>(TParameters parameters)
        where TModel : class, IModel
        where TParameters : IDataSelectParameters<TModel>;

    public Task<bool> SaveChangesAsync(CancellationToken token = default);
}