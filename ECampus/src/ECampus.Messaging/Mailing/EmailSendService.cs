using System.Net.Mail;
using ECampus.Messaging.Options;
using Microsoft.Extensions.Options;

namespace ECampus.Messaging.Mailing;

public class EmailSendService : IEmailSendService
{
    private readonly SmtpClient _smtpClient;
    private readonly IOptions<EmailSetting> _options;

    public EmailSendService(IOptions<EmailSetting> options, SmtpClient smtpClient)
    {
        _options = options;
        _smtpClient = smtpClient;
    }

    public async Task SendEmailAsync(MailMessage email, IEnumerable<string?> receivers)
    {
        email.From = new MailAddress(_options.Value.Email);
        foreach (var receiver in receivers.Where(receiver => !string.IsNullOrWhiteSpace(receiver)))
        {
            email.To.Add(receiver!);
        }

        if (!email.To.Any())
        {
            return;
        }
        await _smtpClient.SendMailAsync(email);
    }
}