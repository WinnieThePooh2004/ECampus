using System.Net.Mail;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.MessageHandlers.TaskSubmissions;
using ECampus.Messaging.Messages;
using Serilog;

namespace ECampus.Tests.Unit.Messaging.MessageHandlers.TaskSubmissions;

public class SubmissionMarkedHandlerTests
{
    private readonly SubmissionMarkedHandler _sut;
    private readonly IEmailSendService _emailSendService = Substitute.For<IEmailSendService>();

    public SubmissionMarkedHandlerTests()
    {
        _sut = new SubmissionMarkedHandler(_emailSendService, Substitute.For<ILogger>());
    }

    [Fact]
    public async Task Handle_ShouldSendEmail()
    {
        var request = new SubmissionMarked { TaskName = "task", UserEmail = "email", ScoredPoints = 1, MaxPoints = 10 };

        await _sut.Handle(request, new CancellationToken());

        await _emailSendService.Received(1).SendEmailAsync(Arg.Is<MailMessage>(message =>
                message.Body == $"<h1>Your submission on task '{request.TaskName}' was marked</h1><p>" +
                $"Your mark is {request.ScoredPoints}/{request.MaxPoints}</p>" &&
                message.Subject == "Your submission was marked"),
            Arg.Is<List<string?>>(l => l.Count == 1 && l.Contains("email")));
    }
}