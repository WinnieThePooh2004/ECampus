using AutoMapper;
using ECampus.Domain.Validation.UniversalValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Validation.UpdateValidators;

public class ClassDtoUpdateValidator : BaseClassDtoValidator, IUpdateValidator<ClassDto>
{
    private readonly IUpdateValidator<ClassDto> _updateValidator;

    public ClassDtoUpdateValidator(IMapper mapper, IValidationDataAccess<Class> dataAccess,
        IUpdateValidator<ClassDto> updateValidator)
        : base(mapper, dataAccess)
    {
        _updateValidator = updateValidator;
    }


    async Task<ValidationResult> IUpdateValidator<ClassDto>.ValidateAsync(ClassDto dataTransferObject)
    {
        var baseErrors = await _updateValidator.ValidateAsync(dataTransferObject);
        if (!baseErrors.IsValid)
        {
            return baseErrors;
        }

        return await base.ValidateAsync(dataTransferObject);
    }
}