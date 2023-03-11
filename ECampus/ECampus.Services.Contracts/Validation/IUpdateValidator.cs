using ECampus.Shared.Validation;

namespace ECampus.Services.Contracts.Validation;

public interface IUpdateValidator<in TDto>
{
    Task<ValidationResult> ValidateAsync(TDto dataTransferObject, CancellationToken token = default);
}