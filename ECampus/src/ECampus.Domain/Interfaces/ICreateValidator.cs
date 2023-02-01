using ECampus.Shared.Validation;

namespace ECampus.Domain.Interfaces;

public interface ICreateValidator<in TDto>
{
    Task<ValidationResult> ValidateAsync(TDto dataTransferObject);
}