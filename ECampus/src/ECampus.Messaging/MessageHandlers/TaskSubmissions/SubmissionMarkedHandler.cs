using System.Net.Mail;
using ECampus.Core.Messages;
using ECampus.Messaging.Mailing;
using MediatR;
using ILogger = Serilog.ILogger;

namespace ECampus.Messaging.MessageHandlers.TaskSubmissions;

public class SubmissionMarkedHandler : IRequestHandler<SubmissionMarked>
{
    private readonly IEmailSendService _emailSendService;
    private readonly ILogger _logger;

    public SubmissionMarkedHandler(IEmailSendService emailSendService, ILogger logger)
    {
        _emailSendService = emailSendService;
        _logger = logger;
    }

    public async Task<Unit> Handle(SubmissionMarked request, CancellationToken cancellationToken)
    {
        Log(request);
        var email = new MailMessage
        {
            Body = $"<h1>Your submission on task '{request.TaskName}' was marked</h1>" +
                   $"<p>Your mark is {request.ScoredPoints}/{request.MaxPoints}</p>",
            Subject = "Your submission was marked",
            IsBodyHtml = true
        };
        await _emailSendService.SendEmailAsync(email, new List<string?> { request.UserEmail });
        return Unit.Value;
    }

    private void Log(SubmissionMarked request)
    {
        _logger.Information("Submission was marked\n" +
                            "User email: {Email};\n" +
                            "Task name: {Name}", request.UserEmail, request.TaskName);
    }
}