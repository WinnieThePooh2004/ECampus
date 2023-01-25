using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;

namespace ECampus.Shared.Interfaces.Domain.Validation;

public interface IParametersValidator<in TParameters> 
    where TParameters : IQueryParameters
{
    Task<ValidationResult> Validate(TParameters parameters);
}