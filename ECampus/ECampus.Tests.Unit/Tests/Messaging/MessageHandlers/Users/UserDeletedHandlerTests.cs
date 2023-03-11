using System.Net.Mail;
using ECampus.Core.Messages;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.MessageHandlers.Users;
using Serilog;

namespace ECampus.Tests.Unit.Tests.Messaging.MessageHandlers.Users;

public class UserDeletedHandlerTests
{
    private readonly UserDeletedHandler _sut;
    private readonly IEmailSendService _emailSendService = Substitute.For<IEmailSendService>();

    public UserDeletedHandlerTests()
    {
        _sut = new UserDeletedHandler(Substitute.For<ILogger>(), _emailSendService);
    }

    [Fact]
    public async Task Handle_ShouldSendEmail()
    {
        var user = new UserDeleted { Username = "username", Email = "email" };

        await _sut.Handle(user, new CancellationToken());

        await _emailSendService.Received(1).SendEmailAsync(Arg.Is<MailMessage>(message =>
                message.Body == "<h2>Your account was deleted</h2>, <p>you can contact us to restore it</p>"
                && message.Subject == "Account was deleted"),
            Arg.Is<List<string?>>(l => l.Count == 1 && l.Contains(user.Email)));
    }
}