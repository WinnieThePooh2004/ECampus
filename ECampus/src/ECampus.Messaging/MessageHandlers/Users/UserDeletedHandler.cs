using System.Net.Mail;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.Messages;
using MediatR;
using ILogger = Serilog.ILogger;

namespace ECampus.Messaging.MessageHandlers.Users;

public class UserDeletedHandler : IRequestHandler<UserDeleted>
{
    private readonly ILogger _logger;
    private readonly IEmailSendService _emailSendService;

    public UserDeletedHandler(ILogger logger, IEmailSendService emailSendService)
    {
        _logger = logger;
        _emailSendService = emailSendService;
    }

    public async Task<Unit> Handle(UserDeleted request, CancellationToken cancellationToken)
    {
        _logger.Information("Deleted user {Username}", request.Username);
        var email = new MailMessage
        {
            Subject = "Account was deleted",
            Body = "<h2>Your account was deleted</h2>, <p>you can contact us to restore it</p>",
            IsBodyHtml = true
        };
        await _emailSendService.SendEmailAsync(email, new List<string> { request.Email });
        return Unit.Value;
    }
}