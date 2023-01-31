using System.Net;
using System.Net.Mail;
using ECampus.Core.Installers;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.Options;
using Microsoft.Extensions.Options;

namespace ECampus.Messaging.Installers;

public class MailingServicesInstaller : IInstaller
{
    public int InstallOrder => 0;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<SmtpClient>(provider =>
        {
            var options = provider.GetService<IOptions<EmailSetting>>();
            return new SmtpClient
            {
                Port = 587,
                Credentials = new NetworkCredential { Password = options!.Value.Password, UserName = options.Value.Email },
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = options.Value.HostName
            };
        });
        services.AddSingleton<IEmailSendService, EmailSendService>();
    }
}