using ECampus.Domain.Validation;

namespace ECampus.Services.Contracts.Validation;

public interface ICreateValidator<in TDto>
{
    Task<ValidationResult> ValidateAsync(TDto dataTransferObject, CancellationToken token = default);
}