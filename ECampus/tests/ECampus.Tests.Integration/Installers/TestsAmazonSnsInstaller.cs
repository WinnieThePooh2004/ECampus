using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using ECampus.Core.Installers;
using ECampus.Tests.Integration.AppFactories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace ECampus.Tests.Integration.Installers;

public class TestsAmazonSnsInstaller : IInstaller
{
    public int InstallOrder => -1;

    private static IAmazonSimpleNotificationService AmazonSnsMock
    {
        get
        {
            if (_amazonSnsMock is not null)
            {
                return _amazonSnsMock;
            }
            
            _amazonSnsMock = Substitute.For<IAmazonSimpleNotificationService>();
            _amazonSnsMock.FindTopicAsync(Arg.Any<string>()).Returns(new Topic { TopicArn = "IntegrationTests" });
            ApplicationFactory.AmazonSnsMock = _amazonSnsMock;
            return _amazonSnsMock;
        }
    }

    private static IAmazonSimpleNotificationService? _amazonSnsMock;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var descriptor = services.Single(service => service.ServiceType == typeof(IAmazonSimpleNotificationService));
        services.Remove(descriptor);
        services.AddSingleton(AmazonSnsMock);
    }
}