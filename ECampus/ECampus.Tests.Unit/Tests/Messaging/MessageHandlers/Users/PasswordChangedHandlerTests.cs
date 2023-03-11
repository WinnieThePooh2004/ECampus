using System.Net.Mail;
using ECampus.Core.Messages;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.MessageHandlers.Users;
using Serilog;

namespace ECampus.Tests.Unit.Tests.Messaging.MessageHandlers.Users;

public class PasswordChangedHandlerTests
{
    private readonly PasswordChangedHandler _sut;
    private readonly IEmailSendService _emailSendService = Substitute.For<IEmailSendService>();

    public PasswordChangedHandlerTests()
    {
        _sut = new PasswordChangedHandler(_emailSendService, Substitute.For<ILogger>());
    }

    [Fact]
    public async Task Handle_ShouldSendEmail()
    {
        var user = new PasswordChanged { Username = "username", Email = "email" };

        await _sut.Handle(user, new CancellationToken());

        await _emailSendService.Received(1).SendEmailAsync(Arg.Is<MailMessage>(message =>
                message.Body == "<h2>Your password was changed</h2><p>If you know about it, just ignore this message</p>" &&
                message.Subject == "Your password was changed"),
            Arg.Is<List<string?>>(l => l.Count == 1 && l.Contains(user.Email)));
    }
}