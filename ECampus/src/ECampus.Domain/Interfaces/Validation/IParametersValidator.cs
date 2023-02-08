using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Interfaces.Validation;

public interface IParametersValidator<in TParameters> 
    where TParameters : IQueryParameters
{
    Task<ValidationResult> ValidateAsync(TParameters parameters, CancellationToken token = default);
}