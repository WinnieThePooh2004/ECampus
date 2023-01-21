using ECampus.Shared.Validation;

namespace ECampus.Shared.Interfaces.Domain.Validation;

public interface IUpdateValidator<in TDto>
{
    Task<ValidationResult> ValidateAsync(TDto dataTransferObject);
}