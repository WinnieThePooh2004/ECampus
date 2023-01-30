﻿using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Metadata;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Services;

[Inject(typeof(IUserService))]
public class UserService : IUserService
{
    private readonly ICreateValidator<UserDto> _createValidator;
    private readonly IUpdateValidator<UserDto> _updateValidator;

    public UserService(IUpdateValidator<UserDto> updateValidator,
        ICreateValidator<UserDto> createValidator)
    {
        _updateValidator = updateValidator;
        _createValidator = createValidator;
    }

    public async Task<ValidationResult> ValidateCreateAsync(UserDto user)
    {
        return await _createValidator.ValidateAsync(user);
    }

    public async Task<ValidationResult> ValidateUpdateAsync(UserDto user)
    {
        return await _updateValidator.ValidateAsync(user);
    }
}