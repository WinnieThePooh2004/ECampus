using ECampus.Services.Contracts.Validation;
using ECampus.Services.Validation.UniversalValidators;
using FluentValidation;

namespace ECampus.Services.Validation.UpdateValidators;

public class UpdateFluentValidatorWrapper<T> : FluentValidatorWrapper<T>, IUpdateValidator<T>
{
    public UpdateFluentValidatorWrapper(IValidator<T> fluentValidationValidator) : base(fluentValidationValidator)
    {
    }
}