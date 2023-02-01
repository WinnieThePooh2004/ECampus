using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;

namespace ECampus.Contracts.DataValidation;

public interface IParametersDataValidator<in TParameters>
    where TParameters : IQueryParameters
{
    Task<ValidationResult> ValidateAsync(TParameters parameters);
}