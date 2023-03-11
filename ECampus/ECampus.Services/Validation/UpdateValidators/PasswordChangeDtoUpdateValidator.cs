using System.Security.Claims;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.Auth;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Services.Validation.UpdateValidators;

public class PasswordChangeDtoUpdateValidator : IUpdateValidator<PasswordChangeDto>
{
    private readonly IUpdateValidator<PasswordChangeDto> _baseValidator;
    private readonly IDataAccessFacade _dataAccess;
    private readonly ClaimsPrincipal _user;

    public PasswordChangeDtoUpdateValidator(IUpdateValidator<PasswordChangeDto> baseValidator,
        IDataAccessFacade dataAccess, IHttpContextAccessor httpContextAccessor)
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