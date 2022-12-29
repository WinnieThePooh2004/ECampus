using FluentValidation;
using UniversityTimetable.Shared.Interfaces.Data.Validation;

namespace UniversityTimetable.Domain.Validation.UpdateValidators;

public class UpdateValidator<TDto> : IUpdateValidator<TDto> 
{
    private readonly IValidator<TDto> _fluentValidationValidator;

    public UpdateValidator(IValidator<TDto> fluentValidationValidator)
    {
        _fluentValidationValidator = fluentValidationValidator;
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateAsync(TDto dataTransferObject) =>
        new((await _fluentValidationValidator.ValidateAsync(dataTransferObject))
            .Errors.Select(error => new KeyValuePair<string,string>(error.PropertyName, error.ErrorMessage)));
}