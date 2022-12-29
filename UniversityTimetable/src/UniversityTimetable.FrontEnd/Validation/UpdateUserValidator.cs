using FluentValidation;
using FluentValidation.Results;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Validation;

public class UpdateUserValidator : IValidator<UserDto>
{
    private readonly IUserRequests _requests;

    public UpdateUserValidator(IUserRequests requests)
    {
        _requests = requests;
    }

    public ValidationResult Validate(IValidationContext context) => new();

    public async Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = new()) 
        => await ValidateAsync((UserDto)context.InstanceToValidate, cancellation);

    public IValidatorDescriptor CreateDescriptor()
        => new ValidatorDescriptor<UserDto>(Array.Empty<IValidationRule<UserDto>>());

    public bool CanValidateInstancesOfType(Type type)
        => type == typeof(UserDto);

    public ValidationResult Validate(UserDto instance) => new();
    
    public async Task<ValidationResult> ValidateAsync(UserDto instance, CancellationToken cancellation = new())
    {
        var result = new ValidationResult();
        var extendedErrors = await _requests.ValidateUpdateAsync(instance);
        result.Errors.AddRange(extendedErrors
            .Select(error => new ValidationFailure(error.Key, error.Value)));
        return result;
    }
}