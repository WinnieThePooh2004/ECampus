using ECampus.Core.Installers;
using ECampus.Messaging.Options;

namespace ECampus.Messaging.Installers;

public class ConfigurationsInstaller : IInstaller
{
    public int InstallOrder => -1;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QueueSettings>(configuration.GetSection(QueueSettings.Key));
        services.Configure<EmailSetting>(configuration.GetSection(EmailSetting.Key));
    }
}