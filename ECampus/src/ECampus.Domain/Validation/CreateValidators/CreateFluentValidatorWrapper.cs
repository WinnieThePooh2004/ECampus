using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Validation.UniversalValidators;
using FluentValidation;

namespace ECampus.Domain.Validation.CreateValidators;

public class CreateFluentValidatorWrapper<T> : FluentValidatorWrapper<T>, ICreateValidator<T>
{
    public CreateFluentValidatorWrapper(IValidator<T> fluentValidationValidator) : base(fluentValidationValidator)
    {
    }
}