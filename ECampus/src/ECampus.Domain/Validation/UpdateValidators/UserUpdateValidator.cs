using System.Security.Claims;
using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Extensions;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Domain.Validation.UpdateValidators;

public class UserUpdateValidator : IUpdateValidator<UserDto>
{
    private readonly IUpdateValidator<UserDto> _updateValidator;
    private readonly IDataValidator<User> _dataAccess;
    private readonly IValidationDataAccess<User> _validationDataAccess;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UserUpdateValidator(IUpdateValidator<UserDto> updateValidator, IMapper mapper,
        IDataValidator<User> dataAccess, IValidationDataAccess<User> validationDataAccess,
        IHttpContextAccessor httpContextAccessor)
    {
        _updateValidator = updateValidator;
        _mapper = mapper;
        _dataAccess = dataAccess;
        _validationDataAccess = validationDataAccess;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ValidationResult> ValidateAsync(UserDto dataTransferObject)
    {
        var errors = await _updateValidator.ValidateAsync(dataTransferObject);
        var model = _mapper.Map<User>(dataTransferObject);
        ValidateRole(dataTransferObject, model, errors);
        errors.MergeResults(await _dataAccess.ValidateUpdate(model));
        var userFromDb = await _validationDataAccess.LoadRequiredDataForUpdateAsync(model);

        if (model.Email != userFromDb.Email)
        {
            errors.AddError(new ValidationError(nameof(model.Email), "You cannot change email"));
        }

        if (model.Password != userFromDb.Password)
        {
            errors.AddError(new ValidationError(nameof(model.Password),
                "To change password use action 'Users/ChangePassword'"));
        }

        return errors;
    }

    private void ValidateRole(UserDto user, User userFromDb, ValidationResult currentErrors)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new HttpContextNotFoundExceptions();
        }

        var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

        if (userFromDb.Role == UserRole.Admin && user.Role != UserRole.Admin &&
            _httpContextAccessor.HttpContext.User.GetId() == user.Id)
        {
            currentErrors.AddError(new ValidationError(nameof(user.Role), "Admin cannon change role for him/herself"));
            return;
        }

        if (user.Role == userFromDb.Role || currentUserRole == nameof(UserRole.Admin))
        {
            return;
        }

        currentErrors.AddError(new ValidationError(nameof(user.Role),
            $"Only admins can change user`s role"));
    }
}