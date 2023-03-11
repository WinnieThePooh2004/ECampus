using System.Net.Mail;
using ECampus.Core.Messages;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.MessageHandlers.Users;
using Serilog;
using Task = System.Threading.Tasks.Task;

namespace ECampus.Tests.Unit.Tests.Messaging.MessageHandlers.Users;

public class UserCreatedHandlerTests
{
    private readonly UserCreatedHandler _sut;
    private readonly IEmailSendService _emailSendService = Substitute.For<IEmailSendService>();

    public UserCreatedHandlerTests()
    {
        _sut = new UserCreatedHandler(Substitute.For<ILogger>(), _emailSendService);
    }

    [Fact]
    public async Task Handle_ShouldSendEmail()
    {
        var user = new UserCreated { Username = "username", Email = "email" };

        await _sut.Handle(user, new CancellationToken());

        await _emailSendService.Received(1).SendEmailAsync(Arg.Is<MailMessage>(message => 
            message.Body == "<h1>Welcome to ECampus service</h1><p>Contact us to get a role or just" +
            " search for timetable as a guest</p>" && message.Subject == "Welcome to ECampus"),
            Arg.Is<List<string?>>(l => l.Count == 1 && l.Contains(user.Email)));
    }
}