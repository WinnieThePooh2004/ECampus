using ECampus.Services.Contracts.Validation;
using ECampus.Services.Validation.UniversalValidators;
using FluentValidation;

namespace ECampus.Services.Validation.CreateValidators;

public class CreateFluentValidatorWrapper<T> : FluentValidatorWrapper<T>, ICreateValidator<T>
{
    public CreateFluentValidatorWrapper(IValidator<T> fluentValidationValidator) : base(fluentValidationValidator)
    {
    }
}