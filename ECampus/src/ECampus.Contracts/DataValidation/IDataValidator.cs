using ECampus.Shared.Validation;

namespace ECampus.Contracts.DataValidation;

public interface IDataValidator<in TModel>
{
    /// <summary>
    /// implement this method ONLY if it cannot be validated on domain level, e. g. username must be unique
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<ValidationResult> ValidateCreate(TModel model);

    /// <summary>
    /// implement this method ONLY if it cannot be validated on domain level, e. g. username must be unique
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<ValidationResult> ValidateUpdate(TModel model);

}