using ECampus.Shared.Validation;
using FluentValidation;

namespace ECampus.Domain.Validation.UniversalValidators;

public abstract class FluentValidatorWrapper<TDto>
{
    private readonly IValidator<TDto> _fluentValidationValidator;

    protected FluentValidatorWrapper(IValidator<TDto> fluentValidationValidator)
    {
        _fluentValidationValidator = fluentValidationValidator;
    }

    public async Task<ValidationResult> ValidateAsync(TDto dataTransferObject) =>
        new((await _fluentValidationValidator.ValidateAsync(dataTransferObject))
            .Errors.Select(error => new ValidationError(error.PropertyName, error.ErrorMessage)));
}