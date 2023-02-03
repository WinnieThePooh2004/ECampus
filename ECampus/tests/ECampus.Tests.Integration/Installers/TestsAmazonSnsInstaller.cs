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

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var descriptor = services.Single(service => service.ServiceType == typeof(IAmazonSimpleNotificationService));
        services.Remove(descriptor);
        var amazonSns = Substitute.For<IAmazonSimpleNotificationService>();
        amazonSns.FindTopicAsync(Arg.Any<string>()).Returns(new Topic { TopicArn = "IntegrationTests" });
        ApplicationFactory.AmazonSnsMock = amazonSns;
        services.AddSingleton(amazonSns);
    }
}