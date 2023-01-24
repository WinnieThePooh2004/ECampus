using Amazon.SQS;
using ECampus.Domain.Messaging;
using ECampus.Shared.Installers;
using ECampus.Shared.Interfaces.Messaging;
using ECampus.Shared.Messaging.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Domain.Installers;

public class MessengersInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISqsMessenger, SqsMessenger>();
        services.Configure<QueueSettings>(configuration.GetSection("Queue"));
        services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
    }
}