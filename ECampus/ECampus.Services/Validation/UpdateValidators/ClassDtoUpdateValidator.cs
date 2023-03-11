﻿using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Validation.UniversalValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Services.Validation.UpdateValidators;

public class ClassDtoUpdateValidator : BaseClassDtoValidator, IUpdateValidator<ClassDto>
{
    private readonly IUpdateValidator<ClassDto> _updateValidator;

    public ClassDtoUpdateValidator(IUpdateValidator<ClassDto> updateValidator,
        IDataAccessFacade parametersDataAccess)
        : base(parametersDataAccess)
    {
        _updateValidator = updateValidator;
    }


    async Task<ValidationResult> IUpdateValidator<ClassDto>.ValidateAsync(ClassDto dataTransferObject, CancellationToken token)
    {
        var baseErrors = await _updateValidator.ValidateAsync(dataTransferObject, token);
        if (!baseErrors.IsValid)
        {
            return baseErrors;
        }

        return await base.ValidateAsync(dataTransferObject);
    }
}