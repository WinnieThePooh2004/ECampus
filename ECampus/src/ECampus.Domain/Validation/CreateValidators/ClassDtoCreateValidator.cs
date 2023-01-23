using AutoMapper;
using ECampus.Domain.Validation.UniversalValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Validation.CreateValidators;

public class ClassDtoCreateValidator : BaseClassDtoValidator, ICreateValidator<ClassDto>
{
    private readonly ICreateValidator<ClassDto> _createValidator;
    
    public ClassDtoCreateValidator(IMapper mapper, IValidationDataAccess<Class> dataAccess, ICreateValidator<ClassDto> createValidator)
        : base(mapper, dataAccess)
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