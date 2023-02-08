using System.Security.Claims;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Domain.Validation.CreateValidators;

public class UserCreateValidator : ICreateValidator<UserDto>
{
    private readonly ICreateValidator<UserDto> _baseValidator;
    private readonly ClaimsPrincipal _user;
    private readonly IDataAccessManager _parametersDataAccess;

    public UserCreateValidator(ICreateValidator<UserDto> baseValidator,
        IHttpContextAccessor httpContextAccessor, IDataAccessManager parametersDataAccess)
    {
        _baseValidator = baseValidator;
        _user = httpContextAccessor.HttpContext!.User;
        _parametersDataAccess = parametersDataAccess;
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
        return errors;
    }

    private async Task ValidateUsernameUniqueness(UserDto dataTransferObject, ValidationResult errors)
    {
        var usersWithSameUsername =
            _parametersDataAccess.GetByParameters<User, UserUsernameParameters>(new UserUsernameParameters
                { Username = dataTransferObject.Username });
        if (await usersWithSameUsername.AnyAsync())
        {
            errors.AddError(new ValidationError(nameof(UserDto.Username), "User with same username already exists"));
        }
    }

    private async Task ValidateEmailUniqueness(string email, ValidationResult errors)
    {
        var usersWithSameEmails =
            _parametersDataAccess.GetByParameters<User, UserEmailParameters>(new UserEmailParameters
                { Email = email });
        if (await usersWithSameEmails.AnyAsync())
        {
            errors.AddError(new ValidationError(nameof(UserDto.Email), "User with same email already exists"));
        }
    }

    private void ValidateRole(UserDto user, ValidationResult currentErrors)
    {
        if (user.Role == UserRole.Guest || _user.IsInRole(nameof(UserRole.Admin)))
        {
            return;
        }

        currentErrors.AddError(new ValidationError(nameof(user.Role),
            $"Only admin can create user with roles different from {nameof(UserRole.Guest)}"));
    }
}