using ECampus.Shared.Data;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataAccess;

public interface IDataAccessManager
{
    Task<TModel> CreateAsync<TModel>(TModel model) where TModel : class, IModel;
    Task<TModel> UpdateAsync<TModel>(TModel model) where TModel : class, IModel;
    Task<TModel> DeleteAsync<TModel>(int id) where TModel : class, IModel, new();
    Task<TModel> GetByIdAsync<TModel>(int id) where TModel : class, IModel;
    IQueryable<TModel> GetByParameters<TModel, TParameters>(TParameters parameters)
        where TModel : class, IModel
        where TParameters : IDataSelectParameters<TModel>;

    public Task<bool> SaveChangesAsync();
}