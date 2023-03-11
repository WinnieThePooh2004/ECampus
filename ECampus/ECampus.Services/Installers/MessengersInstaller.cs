using Amazon.SimpleNotificationService;
using ECampus.Core.Installers;
using ECampus.Services.Contracts.Messaging;
using ECampus.Services.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class MessengersInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISnsMessenger, SnsMessenger>();
        services.Configure<NotificationsSettings>(configuration.GetSection(NotificationsSettings.Key));
        services.Configure<AwsCredentialsSetting>(configuration.GetSection(AwsCredentialsSetting.Key));
        services.AddSingleton<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();
        // _ =>
        // {
        //     var settings = configuration.GetSection(AwsCredentialsSetting.Key).Get<AwsCredentialsSetting>()!;
        //     return new AmazonSimpleNotificationServiceClient(settings.AccessKeyId, settings.SecretKey,
        //         RegionEndpoint.EUCentral1);
        // });
    }
}