using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.Validation;

public interface IValidationDataAccess<TModel>
    where TModel : class, IModel
{
    Task<TModel> LoadRequiredDataForCreate(TModel model);
    Task<TModel> LoadRequiredDataForUpdate(TModel model);
}