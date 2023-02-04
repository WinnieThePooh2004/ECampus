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
    /// should be used only when you want update some objects properties,but don`t want to load all object`s relations;
    /// should not be used in methods like GetByIdAsync or any other methods which results can be seen by user or when;
    /// also this method make direct call to DbSet without any classes between
    /// </summary>
    Task<TModel> GetPureByIdAsync<TModel>(int id) where TModel : class, IModel;

    public Task<bool> SaveChangesAsync();
}