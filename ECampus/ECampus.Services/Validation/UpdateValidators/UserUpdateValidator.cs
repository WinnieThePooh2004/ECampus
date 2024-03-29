﻿using System.Security.Claims;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Domain.Extensions;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Validation.UniversalValidators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Validation.UpdateValidators;

public class UserUpdateValidator : UserValidatorBase, IUpdateValidator<UserDto>
{
    private readonly IUpdateValidator<UserDto> _updateValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataAccessFacade _dataAccess;

    public UserUpdateValidator(IUpdateValidator<UserDto> updateValidator,
        IHttpContextAccessor httpContextAccessor,
        IDataAccessFacade dataAccess) : base(dataAccess)
    {
        _updateValidator = updateValidator;
        _httpContextAccessor = httpContextAccessor;
        _dataAccess = dataAccess;
    }

    public async Task<ValidationResult> ValidateAsync(UserDto dataTransferObject, CancellationToken token = default)
    {
        var errors = await _updateValidator.ValidateAsync(dataTransferObject, token);
        if (!errors.IsValid)
        {
            return errors;
        }
        var userFromDb = await _dataAccess.PureByIdAsync<User>(dataTransferObject.Id, token);
        ValidateRole(dataTransferObject, userFromDb, errors);
        await ValidateUsernameUniqueness(dataTransferObject, errors, token);

        ValidateChanges(dataTransferObject, userFromDb, errors);
        errors.MergeResults(await base.ValidateRole(dataTransferObject, token));
        return errors;
    }

    private static void ValidateChanges(UserDto model, User userFromDb, ValidationResult errors)
    {
        if (model.Email != userFromDb.Email)
        {
            errors.AddError(new ValidationError(nameof(model.Email), "You cannot change email"));
        }
    }

    private async Task ValidateUsernameUniqueness(UserDto dataTransferObject, ValidationResult errors,
        CancellationToken token = default)
    {
        var usersWithSaveUsername =
            _dataAccess.GetByParameters<User, UserUsernameParameters>(
                new UserUsernameParameters(dataTransferObject.Username));
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