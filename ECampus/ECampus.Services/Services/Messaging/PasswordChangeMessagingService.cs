﻿using ECampus.Core.Messages;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Messaging;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Services.Services.Messaging;

public class PasswordChangeMessagingService : IPasswordChangeService
{
    private readonly IPasswordChangeService _baseService;
    private readonly ISnsMessenger _snsMessenger;

    public PasswordChangeMessagingService(IPasswordChangeService baseService, ISnsMessenger snsMessenger)
    {
        _baseService = baseService;
        _snsMessenger = snsMessenger;
    }

    public async Task<UserDto> ChangePassword(PasswordChangeDto passwordChange, CancellationToken token = default)
    {
        var user = await _baseService.ChangePassword(passwordChange, token);
        var message = new PasswordChanged
        {
            Username = user.Username,
            Email = user.Email
        };
        await _snsMessenger.PublishMessageAsync(message);
        return user;
    }

    public Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange, CancellationToken token = default) =>
        _baseService.ValidatePasswordChange(passwordChange, token);
}