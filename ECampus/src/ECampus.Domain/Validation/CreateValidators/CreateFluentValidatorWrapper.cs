using ECampus.Domain.Interfaces;
using ECampus.Domain.Validation.UniversalValidators;
using ECampus.Shared.Interfaces.Domain.Validation;
using FluentValidation;

namespace ECampus.Domain.Validation.CreateValidators;

public class CreateFluentValidatorWrapper<T> : FluentValidatorWrapper<T>, ICreateValidator<T>
{
    public CreateFluentValidatorWrapper(IValidator<T> fluentValidationValidator) : base(fluentValidationValidator)
    {
    }
}