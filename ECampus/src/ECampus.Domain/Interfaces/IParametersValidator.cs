using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Interfaces;

public interface IParametersValidator<in TParameters> 
    where TParameters : IQueryParameters
{
    Task<ValidationResult> ValidateAsync(TParameters parameters);
}