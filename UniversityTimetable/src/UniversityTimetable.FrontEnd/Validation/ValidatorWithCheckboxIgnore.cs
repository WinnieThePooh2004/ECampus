using FluentValidation;
using FluentValidation.Results;
using UniversityTimetable.FrontEnd.Components.Input;

namespace UniversityTimetable.FrontEnd.Validation;

/// <summary>
/// i don`t know why, but after clicking on checkbox in MultipleItemSelect, validators try to validate that checkbox,
/// so this class will check if object type is checkbox and won`t throw exception
/// </summary>
/// <typeparam name="T"></typeparam>
public class ValidatorWithCheckboxIgnore<T> : IValidator<T>
{
    private readonly IValidator<T> _validator;

    public ValidatorWithCheckboxIgnore(IValidator<T> validator)
    {
        _validator = validator;
    }

    public ValidationResult Validate(IValidationContext context) => _validator.Validate(context);

    public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = new())
    {
        if (context.InstanceToValidate is Checkbox)
        {
            return Task.FromResult(new ValidationResult());
        }
        return _validator.ValidateAsync(context, cancellation);
    }

    public IValidatorDescriptor CreateDescriptor() => _validator.CreateDescriptor();

    public bool CanValidateInstancesOfType(Type type) => type == typeof(T) || type == typeof(Checkbox);

    public ValidationResult Validate(T instance) => _validator.Validate(instance);

    public Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellation = new()) 
        => _validator.ValidateAsync(instance, cancellation);
}