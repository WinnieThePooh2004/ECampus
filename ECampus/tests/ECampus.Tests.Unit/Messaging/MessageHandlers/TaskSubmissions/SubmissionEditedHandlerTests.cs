using System.Net.Mail;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.MessageHandlers.TaskSubmissions;
using ECampus.Messaging.MessageHandlers.Users;
using ECampus.Messaging.Messages;
using Serilog;

namespace ECampus.Tests.Unit.Messaging.MessageHandlers.TaskSubmissions;

public class SubmissionEditedHandlerTests
{
    private readonly SubmissionEditedHandler _sut;
    private readonly IEmailSendService _emailSendService = Substitute.For<IEmailSendService>();

    public SubmissionEditedHandlerTests()
    {
        _sut = new SubmissionEditedHandler(_emailSendService, Substitute.For<ILogger>());
    }

    [Fact]
    public async Task Handle_ShouldSendEmail()
    {
        var request = new SubmissionEdited { TaskName = "task", UserEmail = "email" };

        await _sut.Handle(request, new CancellationToken());

        await _emailSendService.Received(1).SendEmailAsync(Arg.Is<MailMessage>(message =>
                message.Body == $"<h2>Your submission on task {request.TaskName} was edited</h2>" &&
                message.Subject == "Submission was edited"),
            Arg.Is<List<string?>>(l => l.Count == 1 && l.Contains("email")));
    }
}