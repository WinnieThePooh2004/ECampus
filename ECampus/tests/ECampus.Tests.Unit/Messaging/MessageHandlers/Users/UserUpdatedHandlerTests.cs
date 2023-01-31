using System.Net.Mail;
using ECampus.Core.Messages;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.MessageHandlers.Users;
using Serilog;

namespace ECampus.Tests.Unit.Messaging.MessageHandlers.Users;

public class UserUpdatedHandlerTests
{
    private readonly UserUpdatedHandler _sut;
    private readonly IEmailSendService _emailSendService = Substitute.For<IEmailSendService>();

    public UserUpdatedHandlerTests()
    {
        _sut = new UserUpdatedHandler(Substitute.For<ILogger>(), _emailSendService);
    }

    [Fact]
    public async Task Handle_ShouldSendEmail()
    {
        var user = new UserUpdated { Username = "username", Email = "email" };

        await _sut.Handle(user, new CancellationToken());

        await _emailSendService.Received(1).SendEmailAsync(Arg.Is<MailMessage>(message => 
                message.Body == "<h1>Your account was updated</h1><p>If you know about it," +
                " just ignore this message</p>"&& message.Subject == "Your account details were changed"),
            Arg.Is<List<string?>>(l => l.Count == 1 && l.Contains(user.Email)));
    }
}