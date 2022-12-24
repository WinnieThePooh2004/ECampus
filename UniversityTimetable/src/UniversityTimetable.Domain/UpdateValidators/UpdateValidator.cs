using FluentValidation;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Domain.UpdateValidators;

public class UpdateValidator<TDto> : IUpdateValidator<TDto> 
    where TDto : class, IDataTransferObject
{
    private readonly IValidator<TDto> _fluentValidationValidator;

    public UpdateValidator(IValidator<TDto> fluentValidationValidator)
    {
        _fluentValidationValidator = fluentValidationValidator;
    }

    public async Task<Dictionary<string, string>> ValidateAsync(TDto dataTransferObject) =>
        new((await _fluentValidationValidator.ValidateAsync(dataTransferObject))
            .Errors.Select(error => new KeyValuePair<string,string>(error.PropertyName, error.ErrorMessage)));
}