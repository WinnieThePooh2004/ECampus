using FluentValidation;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.Domain.Validation.UniversalValidators;

public class FluentValidatorWrapper<TDto> : ICreateValidator<TDto>, IUpdateValidator<TDto>
{
    private readonly IValidator<TDto> _fluentValidationValidator;

    public FluentValidatorWrapper(IValidator<TDto> fluentValidationValidator)
    {
        _fluentValidationValidator = fluentValidationValidator;
    }

    public async Task<ValidationResult> ValidateAsync(TDto dataTransferObject) =>
        new((await _fluentValidationValidator.ValidateAsync(dataTransferObject))
            .Errors.Select(error => new ValidationError(error.PropertyName, error.ErrorMessage)));
}