using ECampus.Contracts.DataAccess;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Validation.UniversalValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Validation.CreateValidators;

public class ClassDtoCreateValidator : BaseClassDtoValidator, ICreateValidator<ClassDto>
{
    private readonly ICreateValidator<ClassDto> _createValidator;

    public ClassDtoCreateValidator(ICreateValidator<ClassDto> createValidator,
        IDataAccessManager parametersDataAccessManager)
        :base(parametersDataAccessManager)
    {
        _createValidator = createValidator;
    }

    async Task<ValidationResult> ICreateValidator<ClassDto>.ValidateAsync(ClassDto dataTransferObject)
    {
        var baseErrors = await _createValidator.ValidateAsync(dataTransferObject);
        if (!baseErrors.IsValid)
        {
            return baseErrors;
        }

        return await base.ValidateAsync(dataTransferObject);
    }
}