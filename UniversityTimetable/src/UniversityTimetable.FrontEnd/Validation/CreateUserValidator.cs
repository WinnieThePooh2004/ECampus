using FluentValidation;
using FluentValidation.Results;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Validation;

public class CreateUserValidator : IValidator<UserDto>
{
    private readonly IValidator<UserDto> _baseValidator;
    private readonly IUserRequests _requests;
    private readonly IHttpContextAccessor _contextAccessor;

    public CreateUserValidator(IValidator<UserDto> baseValidator, IUserRequests requests,
        IHttpContextAccessor contextAccessor)
    {
        _baseValidator = baseValidator;
        _requests = requests;
        _contextAccessor = contextAccessor;
    }

    public ValidationResult Validate(IValidationContext context)
        => _baseValidator.Validate(context);

    public async Task<ValidationResult> ValidateAsync(IValidationContext context,
        CancellationToken cancellation = new())
        => await ValidateAsync((UserDto)context.InstanceToValidate, cancellation);

    public IValidatorDescriptor CreateDescriptor()
        => _baseValidator.CreateDescriptor();

    public bool CanValidateInstancesOfType(Type type)
        => _baseValidator.CanValidateInstancesOfType(type);

    public ValidationResult Validate(UserDto instance)
        => _baseValidator.Validate(instance);

    public async Task<ValidationResult> ValidateAsync(UserDto instance, CancellationToken cancellation = new())
    {
        var baseErrors = await _baseValidator.ValidateAsync(instance, cancellation);
        if (instance.Role == UserRole.Admin
            && !(_contextAccessor?.HttpContext?.User.IsInRole(nameof(UserRole.Admin)) ?? false))
        {
            baseErrors.Errors.Add(new ValidationFailure(nameof(instance.Role),
                "Cannot create new admin unless yor are admin"));
        }

        if (baseErrors.Errors.Any())
        {
            return baseErrors;
        }

        var extendedErrors = await _requests.ValidateCreateAsync(instance);
        baseErrors.Errors.AddRange(extendedErrors
            .Select(error => new ValidationFailure(error.Key, error.Value)));
        return baseErrors;
    }
}