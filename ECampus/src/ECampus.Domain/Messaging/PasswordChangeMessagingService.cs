﻿using ECampus.Contracts.Services;
using ECampus.Core.Messages;
using ECampus.Domain.Interfaces;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Messaging;

public class PasswordChangeMessagingService : IPasswordChangeService
{
    private readonly IPasswordChangeService _baseService;
    private readonly ISnsMessenger _snsMessenger;

    public PasswordChangeMessagingService(IPasswordChangeService baseService, ISnsMessenger snsMessenger)
    {
        _baseService = baseService;
        _snsMessenger = snsMessenger;
    }

    public async Task<UserDto> ChangePassword(PasswordChangeDto passwordChange)
    {
        var user = await _baseService.ChangePassword(passwordChange);
        var message = new PasswordChanged
        {
            Username = user.Username,
            Email = user.Email
        };
        await _snsMessenger.PublishMessageAsync(message);
        return user;
    }

    public Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange) =>
        _baseService.ValidatePasswordChange(passwordChange);
}