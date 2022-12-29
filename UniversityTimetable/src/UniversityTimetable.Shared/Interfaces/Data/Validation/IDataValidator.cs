using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.Validation;

public interface IDataValidator<TModel> : IValidationDataAccess<TModel>
    where TModel : class, IModel
{
    /// <summary>
    /// implement this method ONLY if it cannot be validated on domain level, e. g. username must be unique
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Dictionary<string, string>> ValidateCreate(TModel model);

    /// <summary>
    /// implement this method ONLY if it cannot be validated on domain level, e. g. username must be unique
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Dictionary<string, string>> ValidateUpdate(TModel model);

}