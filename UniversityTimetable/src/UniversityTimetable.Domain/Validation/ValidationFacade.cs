using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.Data.Validation;

namespace UniversityTimetable.Domain.Validation;

public class ValidationFacade<T> : IValidationFacade<T>
    where T : class, IDataTransferObject
{
    private readonly IUpdateValidator<T> _updateValidator;
    private readonly ICreateValidator<T> _createValidator;

    public ValidationFacade(IUpdateValidator<T> updateValidator, ICreateValidator<T> createValidator)
    {
        _updateValidator = updateValidator;
        _createValidator = createValidator;
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateCreate(T instance)
    {
        var errors = await _createValidator.ValidateAsync(instance);
        if (instance.Id != 0)
        {
            errors.Add(KeyValuePair.Create(nameof(instance.Id), "Cannot add object to database if its id is not 0"));
        }

        return errors;
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateUpdate(T instance)
    {
        var errors = await _updateValidator.ValidateAsync(instance);
        if (instance.Id < 0)
        {
            errors.Add(KeyValuePair.Create(nameof(instance.Id), "Cannot update object if its id is less or equal 0"));
        }

        return errors;
    }
}