using System.Net.Mail;
using ECampus.Core.Messages;
using ECampus.Messaging.Mailing;
using MediatR;
using ILogger = Serilog.ILogger;

namespace ECampus.Messaging.MessageHandlers.TaskSubmissions;

public class SubmissionEditedHandler : IRequestHandler<SubmissionEdited>
{
    private readonly IEmailSendService _emailSendService;
    private readonly ILogger _logger;

    public SubmissionEditedHandler(IEmailSendService emailSendService, ILogger logger)
    {
        _emailSendService = emailSendService;
        _logger = logger;
    }

    public async Task<Unit> Handle(SubmissionEdited request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserEmail))
        {
            _logger.Warning("User email is null or empty, cannot send email");
            return Unit.Value;
        }

        Log(request);
        var email = new MailMessage
        {
            Body = $"<h2>Your submission on task {request.TaskName} was edited</h2>",
            Subject = "Submission was edited",
            IsBodyHtml = true
        };
        await _emailSendService.SendEmailAsync(email, new List<string> { request.UserEmail });
        return Unit.Value;
    }

    private void Log(SubmissionEdited request)
    {
        _logger.Information("Submission was edited:\n" +
                            "Student`s email: {Email}\n" +
                            "Task name: {Name}", request.UserEmail, request.TaskName);
    }
}