using ECampus.Shared.Data;

namespace ECampus.Contracts.DataValidation;

public interface IValidationDataAccess<TModel>
    where TModel : class, IModel
{
    Task<TModel> LoadRequiredDataForCreateAsync(TModel model);
    Task<TModel> LoadRequiredDataForUpdateAsync(TModel model);
}