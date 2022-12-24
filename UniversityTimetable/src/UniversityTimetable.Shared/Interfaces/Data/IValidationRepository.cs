namespace UniversityTimetable.Shared.Interfaces.Data;

public interface IValidationRepository<TModel>
    where TModel : class, IModel
{
    Task<TModel> LoadRequiredDataForCreate(TModel model);
    Task<TModel> LoadRequiredDataForUpdate(TModel model);

    /// <summary>
    /// implement this method ONLY if it cannot be validated on domain level, e. g. username must be unique
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Dictionary<string, string>> ValidateCreateOnDataBaseLevel(TModel model) => Task.FromResult(new Dictionary<string, string>());

    /// <summary>
    /// implement this method ONLY if it cannot be validated on domain level, e. g. username must be unique
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Dictionary<string, string>> ValidateUpdateOnDataBaseLevel(TModel model)  => Task.FromResult(new Dictionary<string, string>());
}