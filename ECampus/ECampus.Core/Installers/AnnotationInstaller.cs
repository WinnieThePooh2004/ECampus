using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Core.Installers;

public class AnnotationInstaller : IInstaller
{
    private readonly Assembly _assembly;
    public int InstallOrder => -1;

    public AnnotationInstaller(Assembly assembly)
    {
        _assembly = assembly;
    }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var typesToInstall = _assembly.GetTypes()
            .Where(type => type.GetCustomAttributes(typeof(InjectAttribute), false).Any())
            .Select(type => (type, type.GetCustomAttributes(false).OfType<InjectAttribute>().Single()))
            .Select(type => new ServiceDescriptor(type.Item2.ServiceType, 
                type.type, type.Item2.ServiceLifetime));

        foreach (var serviceDescriptor in typesToInstall)
        {
            services.Add(serviceDescriptor);
        }
    }
}