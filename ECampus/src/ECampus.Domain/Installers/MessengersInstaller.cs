using Amazon.SimpleNotificationService;
using ECampus.Core.Installers;
using ECampus.Domain.Interfaces;
using ECampus.Domain.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Domain.Installers;

public class MessengersInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISnsMessenger, SnsMessenger>();
        services.Configure<NotificationsSettings>(configuration.GetSection(NotificationsSettings.Key));
        services.AddSingleton<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();
    }
}