using ECampus.Shared.Validation;

namespace ECampus.Domain.Interfaces.Validation;

public interface IUpdateValidator<in TDto>
{
    Task<ValidationResult> ValidateAsync(TDto dataTransferObject, CancellationToken token = default);
}