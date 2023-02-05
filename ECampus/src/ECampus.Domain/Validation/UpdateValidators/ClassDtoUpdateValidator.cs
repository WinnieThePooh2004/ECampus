﻿using ECampus.Contracts.DataAccess;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Validation.UniversalValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Validation.UpdateValidators;

public class ClassDtoUpdateValidator : BaseClassDtoValidator, IUpdateValidator<ClassDto>
{
    private readonly IUpdateValidator<ClassDto> _updateValidator;

    public ClassDtoUpdateValidator(IUpdateValidator<ClassDto> updateValidator,
        IParametersDataAccessManager parametersDataAccess)
        : base(parametersDataAccess)
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