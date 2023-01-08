using FluentValidation;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;

namespace UniversityTimetable.Domain.Validation.CreateValidators;

public class CreateValidator<TDto> : ICreateValidator<TDto>
    where TDto : class
{
    private readonly IValidator<TDto> _fluentValidationValidator;

    public CreateValidator(IValidator<TDto> fluentValidationValidator)
    {
        _fluentValidationValidator = fluentValidationValidator;
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateAsync(TDto dataTransferObject) =>
        new((await _fluentValidationValidator.ValidateAsync(dataTransferObject))
            .Errors.Select(error => new KeyValuePair<string,string>(error.PropertyName, error.ErrorMessage)));

}