using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Validation.UniversalValidators;
using FluentValidation;

namespace ECampus.Domain.Validation.UpdateValidators;

public class UpdateFluentValidatorWrapper<T> : FluentValidatorWrapper<T>, IUpdateValidator<T>
{
    public UpdateFluentValidatorWrapper(IValidator<T> fluentValidationValidator) : base(fluentValidationValidator)
    {
    }
}