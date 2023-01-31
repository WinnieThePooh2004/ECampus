using System.Net.Mail;
using ECampus.Core.Messages;
using ECampus.Messaging.Mailing;
using MediatR;
using ILogger = Serilog.ILogger;

namespace ECampus.Messaging.MessageHandlers.Users;

public class UserCreatedHandler : IRequestHandler<UserCreated>
{
    private readonly ILogger _logger;
    private readonly IEmailSendService _emailSendService;

    public UserCreatedHandler(ILogger logger, IEmailSendService emailSendService)
    {
        _logger = logger;
        _emailSendService = emailSendService;
    }

    public async Task<Unit> Handle(UserCreated request, CancellationToken cancellationToken)
    {
        var email = new MailMessage
        {
            Subject = "Welcome to ECampus",
            Body = "<h1>Welcome to ECampus service</h1>" +
                   "<p>Contact us to get a role or just search for timetable as a guest</p>",
            IsBodyHtml = true
        };
        await _emailSendService.SendEmailAsync(email, new List<string> { request.Email });
        _logger.Information("Created new user {Username}", request.Username);
        return Unit.Value;
    }
}