using System.Net.Mail;
using ECampus.Core.Messages;
using ECampus.Messaging.Mailing;
using MediatR;
using ILogger = Serilog.ILogger;

namespace ECampus.Messaging.MessageHandlers.Users;

public class PasswordChangedHandler : IRequestHandler<PasswordChanged>
{
    private readonly IEmailSendService _emailSendService;
    private readonly ILogger _logger;

    public PasswordChangedHandler(IEmailSendService emailSendService, ILogger logger)
    {
        _emailSendService = emailSendService;
        _logger = logger;
    }

    public async Task<Unit> Handle(PasswordChanged request, CancellationToken cancellationToken)
    {
        Log(request);
        var email = new MailMessage
        {
            Subject = "Your password was changed",
            Body = "<h2>Your password was changed</h2><p>If you know about it, just ignore this message</p>",
            IsBodyHtml = true
        };

        await _emailSendService.SendEmailAsync(email, new List<string?> { request.Email });
        return Unit.Value;
    }

    private void Log(PasswordChanged message)
    {
        _logger.Information("{Username}'s password was changed", message.Username);
    }
}