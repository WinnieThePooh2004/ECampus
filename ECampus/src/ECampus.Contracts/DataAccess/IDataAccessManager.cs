using ECampus.Shared.Data;

namespace ECampus.Contracts.DataAccess;

public interface IDataAccessManager
{
    Task<TModel> CreateAsync<TModel>(TModel model) where TModel : class, IModel;
    Task<TModel> UpdateAsync<TModel>(TModel model) where TModel : class, IModel;
    Task<TModel> DeleteAsync<TModel>(int id) where TModel : class, IModel, new();
    Task<TModel> GetByIdAsync<TModel>(int id) where TModel : class, IModel;

    public Task<bool> SaveChangesAsync();
}