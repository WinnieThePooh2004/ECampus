using FluentValidation;
using FluentValidation.Results;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Validation
{
    public class ExtendedClassDtoValidator : IValidator<ClassDto>
    {
        private readonly IValidator<ClassDto> _baseValidator;
        private readonly IClassRequests _requests;

        public ExtendedClassDtoValidator(IValidator<ClassDto> baseValidator, IClassRequests requests)
        {
            _baseValidator = baseValidator;
            _requests = requests;
        }

        public bool CanValidateInstancesOfType(Type type)
            => _baseValidator.CanValidateInstancesOfType(type);

        public IValidatorDescriptor CreateDescriptor()
            => _baseValidator.CreateDescriptor();

        public ValidationResult Validate(ClassDto instance)
            => _baseValidator.Validate(instance);

        public ValidationResult Validate(IValidationContext context)
            => _baseValidator.Validate(context);

        public async Task<ValidationResult> ValidateAsync(ClassDto instance, CancellationToken cancellation = default)
        {
            var baseResult = await _baseValidator.ValidateAsync(instance, cancellation);
            if (baseResult.Errors.Any())
            {
                return baseResult;
            }

            baseResult.Errors.AddRange(
                (await _requests.ValidateAsync(instance)).Select(error =>
                    new ValidationFailure(error.Key, error.Value)));
            return baseResult;
        }

        public async Task<ValidationResult> ValidateAsync(IValidationContext context,
            CancellationToken cancellation = default)
        {
            var baseResult = await _baseValidator.ValidateAsync(context, cancellation);
            if (baseResult.Errors.Any())
            {
                return baseResult;
            }

            baseResult.Errors.AddRange((await _requests.ValidateAsync((ClassDto)context.InstanceToValidate))
                .Select(error => new ValidationFailure(error.Key, error.Value)));
            return baseResult;
        }
    }
}