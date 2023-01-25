using System.Net.Mail;

namespace ECampus.Messaging.Mailing;

public interface IEmailSendService
{
    Task SendEmailAsync(MailMessage email, params string[] receivers);
}