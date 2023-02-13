﻿using System.Security.Claims;
using ECampus.Contracts.DataAccess;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Domain.Validation.UpdateValidators;

public class PasswordChangeDtoUpdateValidator : IUpdateValidator<PasswordChangeDto>
{
    private readonly IUpdateValidator<PasswordChangeDto> _baseValidator;
    private readonly IDataAccessManager _dataAccess;
    private readonly ClaimsPrincipal _user;

    public PasswordChangeDtoUpdateValidator(IUpdateValidator<PasswordChangeDto> baseValidator,
        IDataAccessManager dataAccess, IHttpContextAccessor httpContextAccessor)
    {
        _baseValidator = baseValidator;
        _dataAccess = dataAccess;
        _user = httpContextAccessor.HttpContext!.User;
    }

    public async Task<ValidationResult> ValidateAsync(PasswordChangeDto dataTransferObject, CancellationToken token = default)
    {
        var errors = await _baseValidator.ValidateAsync(dataTransferObject, token);
        var idValidationResults = ValidateRole(dataTransferObject.UserId);
        errors.MergeResults(idValidationResults);
        if (!idValidationResults.IsValid)
        {
            return errors;
        }
        var user = await _dataAccess.PureByIdAsync<User>(dataTransferObject.UserId, token);
        if (user.Password != dataTransferObject.OldPassword)
        {
            errors.AddError(new ValidationError(nameof(dataTransferObject.OldPassword), "Invalid old password"));
        }
        return errors;
    }

    private ValidationResult ValidateRole(int userId)
    {
        var userIdValidation = _user.ValidateParsableClaim<int>(CustomClaimTypes.Id);
        if (userIdValidation.ClaimValue != userId)
        {
            userIdValidation.Result.AddError(new ValidationError(nameof(PasswordChangeDto.UserId), 
                "You must be account owner or admin to change this user`s password"));
        }

        return userIdValidation.Result;
    }
}