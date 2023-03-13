using ECampus.Domain.Requests;
using ECampus.Domain.Validation;

namespace ECampus.Services.Contracts.Validation;

public interface IParametersValidator<in TParameters> 
    where TParameters : IQueryParameters
{
    Task<ValidationResult> ValidateAsync(TParameters parameters, CancellationToken token = default);
}