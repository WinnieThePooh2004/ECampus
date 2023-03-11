using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Validation.UniversalValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Services.Validation.CreateValidators;

public class ClassDtoCreateValidator : BaseClassDtoValidator, ICreateValidator<ClassDto>
{
    private readonly ICreateValidator<ClassDto> _createValidator;

    public ClassDtoCreateValidator(ICreateValidator<ClassDto> createValidator,
        IDataAccessFacade parametersDataAccessFacade)
        :base(parametersDataAccessFacade)
    {
        _createValidator = createValidator;
    }

    async Task<ValidationResult> ICreateValidator<ClassDto>.ValidateAsync(ClassDto dataTransferObject, CancellationToken token)
    {
        var baseErrors = await _createValidator.ValidateAsync(dataTransferObject, token);
        if (!baseErrors.IsValid)
        {
            return baseErrors;
        }

        return await base.ValidateAsync(dataTransferObject);
    }
}