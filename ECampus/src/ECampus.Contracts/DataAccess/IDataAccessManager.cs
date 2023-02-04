using ECampus.Shared.Data;

namespace ECampus.Contracts.DataAccess;

public interface IDataAccessManager
{
    Task<TModel> CreateAsync<TModel>(TModel model) where TModel : class, IModel;
    Task<TModel> UpdateAsync<TModel>(TModel model) where TModel : class, IModel;
    Task<TModel> DeleteAsync<TModel>(int id) where TModel : class, IModel, new();
    /// <summary>
    /// returns value found by ISingleItemSelector of TModel, can contains any data logic
    /// </summary>
    Task<TModel> GetByIdAsync<TModel>(int id) where TModel : class, IModel;
    /// <summary>
    /// always returns single object (or throws exception) without any navigation properties, its logic cannot be changed;
    /// use this method when you need to get tracked object, but don`t want to load any relations, e. g. for update user`s password
    /// </summary>
    Task<TModel> GetPureByIdAsync<TModel>(int id) where TModel : class, IModel;

    public Task<bool> SaveChangesAsync();
}