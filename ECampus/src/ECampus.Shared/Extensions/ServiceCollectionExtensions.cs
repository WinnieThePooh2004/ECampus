using System.Reflection;
using ECampus.Shared.Installers;
using ECampus.Shared.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static void InstallServices<TAssemblyMarker>(this IServiceCollection services, IConfiguration configuration)
        where TAssemblyMarker : IAssemblyMarker
    {
        services.InstallServices(configuration, typeof(TAssemblyMarker));
    }
    
    public static void InstallServices(this IServiceCollection services, IConfiguration configuration, params Type[] assemblyMarkers)
    {
        services.InstallServices(configuration, assemblyMarkers.Select(marker => marker.Assembly).ToArray());
    }

    private static void InstallServices(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
    {
        var installers = assemblies.SelectMany(a => a.GetTypes())
            .Where(type => type.IsAssignableTo(typeof(IInstaller)) && type is { IsClass: true, IsAbstract: false })
            .Select(type => (IInstaller)Activator.CreateInstance(type)!)
            .OrderBy(installer => installer.InstallOrder)
            .ToList();

        foreach (var installer in installers)
        {
            installer.Install(services, configuration);
        }
    }
    
    public static void AddUniqueServices(this IServiceCollection services, params Type[] assemblyMarkers)
    {
        services.AddUniqueServices(assemblyMarkers.Select(marker => marker.Assembly).ToArray());
    }

    private static void AddUniqueServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddRange(assemblies.SelectMany(assembly => assembly.GetTypes().Where(type =>
                type.GetCustomAttributes().Any(attribute => attribute.GetType() == typeof(InjectAttribute))))
            .SelectMany(type => type.GetCustomAttributes().OfType<InjectAttribute>().Select(attribute =>
                new ServiceDescriptor(attribute.ServiceType, type, attribute.ServiceLifetime))));
    }

    private static void AddRange(this IServiceCollection services, IEnumerable<ServiceDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            services.Add(descriptor);
        }
    }
}