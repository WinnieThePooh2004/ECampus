﻿using System.Net.Mail;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.Messaging.Users;
using ILogger = Serilog.ILogger;
using MediatR;

namespace ECampus.Messaging.MessageHandlers.Users;

public class UserUpdatedHandler : IRequestHandler<UserUpdated>
{
    private readonly ILogger _logger;
    private readonly IEmailSendService _emailSendService;

    public UserUpdatedHandler(ILogger logger, IEmailSendService emailSendService)
    {
        _logger = logger;
        _emailSendService = emailSendService;
    }

    public async Task<Unit> Handle(UserUpdated request, CancellationToken cancellationToken)
    {
        _logger.Information("Updated user {Username}", request.Username);
        var email = new MailMessage
        {
            Subject = "Welcome to ECampus",
            Body = "<h1>Your account was updated</h1>" +
                   "<p>If you know about it, just ignore this message</p>",
            IsBodyHtml = true
        };
        await _emailSendService.SendEmailAsync(email, request.Email);
        return Unit.Value;
    }
}