using System.Net;
using System.Net.Mail;
using ECampus.Messaging.Options;
using Microsoft.Extensions.Options;

namespace ECampus.Messaging.Mailing;

public class EmailSendService : IEmailSendService
{
    private readonly SmtpClient _smtpClient;
    private readonly IOptions<EmailSetting> _options;

    public EmailSendService(IOptions<EmailSetting> options)
    {
        _options = options;
        _smtpClient = new SmtpClient(options.Value.HostName)
        {
            Port = 587,
            Credentials = new NetworkCredential { Password = options.Value.Password, UserName = options.Value.Email },
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };
    }

    public async Task SendEmailAsync(MailMessage email, params string[] receivers)
    {
        email.From = new MailAddress(_options.Value.Email);
        foreach (var receiver in receivers)
        {
            email.To.Add(receiver);
        }
        await _smtpClient.SendMailAsync(email);
    }
}