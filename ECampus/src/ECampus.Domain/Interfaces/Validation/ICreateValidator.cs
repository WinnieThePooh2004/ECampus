using ECampus.Shared.Validation;

namespace ECampus.Domain.Interfaces.Validation;

public interface ICreateValidator<in TDto>
{
    Task<ValidationResult> ValidateAsync(TDto dataTransferObject, CancellationToken token = default);
}