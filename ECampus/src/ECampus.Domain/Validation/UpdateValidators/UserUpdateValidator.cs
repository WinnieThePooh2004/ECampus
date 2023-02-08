using System.Security.Claims;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Domain.Validation.UpdateValidators;

public class UserUpdateValidator : IUpdateValidator<UserDto>
{
    private readonly IUpdateValidator<UserDto> _updateValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataAccessManager _dataAccess;

    public UserUpdateValidator(IUpdateValidator<UserDto> updateValidator,
        IHttpContextAccessor httpContextAccessor,
        IDataAccessManagerFactory dataAccess)
    {
        _updateValidator = updateValidator;
        _httpContextAccessor = httpContextAccessor;
        _dataAccess = dataAccess.Primitive;
    }

    public async Task<ValidationResult> ValidateAsync(UserDto dataTransferObject, CancellationToken token = default)
    {
        var errors = await _updateValidator.ValidateAsync(dataTransferObject, token);
        var userFromDb = await _dataAccess.GetByIdAsync<User>(dataTransferObject.Id, token);
        ValidateRole(dataTransferObject, userFromDb, errors);
        await ValidateUsernameUniqueness(dataTransferObject, errors, token);
        
        ValidateChanges(dataTransferObject, userFromDb, errors);

        return errors;
    }

    private static void ValidateChanges(UserDto model, User userFromDb, ValidationResult errors)
    {
        if (model.Email != userFromDb.Email)
        {
            errors.AddError(new ValidationError(nameof(model.Email), "You cannot change email"));
        }

        if (model.Password != userFromDb.Password)
        {
            errors.AddError(new ValidationError(nameof(model.Password),
                "To change password use action 'Users/ChangePassword'"));
        }
    }

    private async Task ValidateUsernameUniqueness(UserDto dataTransferObject, ValidationResult errors, CancellationToken token = default)
    {
        var usersWithSaveUsername =
            _dataAccess.GetByParameters<User, UserUsernameParameters>(new UserUsernameParameters
                { Username = dataTransferObject.Username });
        if (await usersWithSaveUsername.AnyAsync(user => user.Id != dataTransferObject.Id, token))
        {
            errors.AddError(new ValidationError(nameof(dataTransferObject.Username),
                "User with this username already exists"));
        }
    }

    private void ValidateRole(UserDto user, User userFromDb, ValidationResult currentErrors)
    {
        if (userFromDb.Role == UserRole.Admin && user.Role != UserRole.Admin &&
            _httpContextAccessor.HttpContext!.User.GetId() == user.Id)
        {
            currentErrors.AddError(new ValidationError(nameof(user.Role), "Admin cannon change role for him/herself"));
            return;
        }

        var currentUserRole = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Role)?.Value;

        if (user.Role == userFromDb.Role || currentUserRole == nameof(UserRole.Admin))
        {
            return;
        }

        currentErrors.AddError(new ValidationError(nameof(user.Role),
            "Only admins can change user`s role"));
    }
}