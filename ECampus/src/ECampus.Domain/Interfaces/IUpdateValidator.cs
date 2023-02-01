using ECampus.Shared.Validation;

namespace ECampus.Domain.Interfaces;

public interface IUpdateValidator<in TDto>
{
    Task<ValidationResult> ValidateAsync(TDto dataTransferObject);
}