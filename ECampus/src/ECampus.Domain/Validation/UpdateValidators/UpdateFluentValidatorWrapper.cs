using ECampus.Domain.Interfaces;
using ECampus.Domain.Validation.UniversalValidators;
using ECampus.Shared.Interfaces.Domain.Validation;
using FluentValidation;

namespace ECampus.Domain.Validation.UpdateValidators;

public class UpdateFluentValidatorWrapper<T> : FluentValidatorWrapper<T>, IUpdateValidator<T>
{
    public UpdateFluentValidatorWrapper(IValidator<T> fluentValidationValidator) : base(fluentValidationValidator)
    {
    }
}