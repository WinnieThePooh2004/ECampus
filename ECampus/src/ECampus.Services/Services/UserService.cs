﻿using ECampus.Contracts.Services;
using ECampus.Core.Installers;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Services.Services;

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

    public async Task<ValidationResult> ValidateCreateAsync(UserDto user, CancellationToken token = default)
    {
        return await _createValidator.ValidateAsync(user, token);
    }

    public async Task<ValidationResult> ValidateUpdateAsync(UserDto user, CancellationToken token = default)
    {
        return await _updateValidator.ValidateAsync(user, token);
    }
}