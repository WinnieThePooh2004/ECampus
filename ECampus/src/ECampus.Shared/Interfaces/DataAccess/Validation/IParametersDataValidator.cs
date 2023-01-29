using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;

namespace ECampus.Shared.Interfaces.DataAccess.Validation;

public interface IParametersDataValidator<in TParameters>
    where TParameters : IQueryParameters
{
    Task<ValidationResult> ValidateAsync(TParameters parameters);
}