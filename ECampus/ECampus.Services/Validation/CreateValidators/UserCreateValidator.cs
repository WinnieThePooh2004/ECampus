using System.Security.Claims;
using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Models;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Validation.UniversalValidators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Validation.CreateValidators;

public class UserCreateValidator : UserValidatorBase, ICreateValidator<UserDto>
{
    private readonly ICreateValidator<UserDto> _baseValidator;
    private readonly ClaimsPrincipal _user;
    private readonly IDataAccessFacade _dataAccess;

    public UserCreateValidator(ICreateValidator<UserDto> baseValidator,
        IHttpContextAccessor httpContextAccessor, IDataAccessFacade dataAccess) : base(dataAccess)
    {
        _baseValidator = baseValidator;
        _user = httpContextAccessor.HttpContext!.User;
        _dataAccess = dataAccess;
    }

    public async Task<ValidationResult> ValidateAsync(UserDto dataTransferObject, CancellationToken token = default)
    {
        var errors = await _baseValidator.ValidateAsync(dataTransferObject, token);
        ValidateRole(dataTransferObject, errors);
        if (!errors.IsValid)
        {
            return errors;
        }

        await ValidateEmailUniqueness(dataTransferObject.Email, errors);
        await ValidateUsernameUniqueness(dataTransferObject, errors);
        errors.MergeResults(await base.ValidateRole(dataTransferObject, token));
        return errors;
    }

    private async Task ValidateUsernameUniqueness(UserDto dataTransferObject, ValidationResult errors)
    {
        var usersWithSameUsername =
            _dataAccess.GetByParameters<User, UserUsernameParameters>(
                new UserUsernameParameters(dataTransferObject.Username));
        if (await usersWithSameUsername.AnyAsync())
        {
            errors.AddError(new ValidationError(nameof(UserDto.Username), "User with same username already exists"));
        }
    }

    private async Task ValidateEmailUniqueness(string email, ValidationResult errors)
    {
        var usersWithSameEmails =
            _dataAccess.GetByParameters<User, UserEmailParameters>(new UserEmailParameters(email));
        if (await usersWithSameEmails.AnyAsync())
        {
            errors.AddError(new ValidationError(nameof(UserDto.Email), "User with same email already exists"));
        }
    }

    private void ValidateRole(UserDto user, ValidationResult currentErrors)
    {
        if (user.Role != UserRole.Guest && !_user.IsInRole(nameof(UserRole.Admin)))
        {
            currentErrors.AddError(new ValidationError(nameof(user.Role),
                $"Only admin can create user with roles different from {nameof(UserRole.Guest)}"));
        }
    }
}