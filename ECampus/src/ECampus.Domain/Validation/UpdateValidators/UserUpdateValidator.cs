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
        ValidateRole(dataTransferObject, errors);
        var model = _mapper.Map<User>(dataTransferObject);
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
    
    private void ValidateRole(UserDto user, ValidationResult currentErrors)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new HttpContextNotFoundExceptions();
        }

        if (user.Role == UserRole.Guest || _httpContextAccessor.HttpContext.User.IsInRole(nameof(UserRole.Admin)))
        {
            return;
        }

        currentErrors.AddError(new ValidationError(nameof(user.Role),
            $"Only admin can create user with roles different from {nameof(UserRole.Guest)}"));
    }
}