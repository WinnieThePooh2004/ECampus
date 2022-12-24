using FluentValidation;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Domain.CreateValidators;

public class CreateValidator<TDto> : ICreateValidator<TDto>
    where TDto : class, IDataTransferObject
{
    private readonly IValidator<TDto> _fluentValidationValidator;

    public CreateValidator(IValidator<TDto> fluentValidationValidator)
    {
        _fluentValidationValidator = fluentValidationValidator;
    }

    public async Task<Dictionary<string, string>> ValidateAsync(TDto dataTransferObject) =>
        new((await _fluentValidationValidator.ValidateAsync(dataTransferObject))
            .Errors.Select(error => new KeyValuePair<string,string>(error.PropertyName, error.ErrorMessage)));

}