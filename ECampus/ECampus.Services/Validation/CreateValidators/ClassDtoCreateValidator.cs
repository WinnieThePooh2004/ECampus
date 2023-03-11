using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Validation.UniversalValidators;

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