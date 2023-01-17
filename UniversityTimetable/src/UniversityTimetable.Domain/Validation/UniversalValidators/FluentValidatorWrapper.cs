using FluentValidation;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;

namespace UniversityTimetable.Domain.Validation.UniversalValidators;

public class FluentValidatorWrapper<TDto> : ICreateValidator<TDto>, IUpdateValidator<TDto>
{
    private readonly IValidator<TDto> _fluentValidationValidator;

    public FluentValidatorWrapper(IValidator<TDto> fluentValidationValidator)
    {
        _fluentValidationValidator = fluentValidationValidator;
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateAsync(TDto dataTransferObject) =>
        new((await _fluentValidationValidator.ValidateAsync(dataTransferObject))
            .Errors.Select(error => new KeyValuePair<string,string>(error.PropertyName, error.ErrorMessage)));
}