using System.Collections.Immutable;
using System.Reflection;
using ECampus.Core.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddLazy<TService, TImplementation>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptor = new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime);
        services.Add(descriptor);
        services.AddLazy<TService>();
    }

    public static void AddLazy<T>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var descriptor = new ServiceDescriptor(typeof(Lazy<T>), provider => new Lazy<T>(provider.GetServiceOfType<T>),
            serviceLifetime);
        services.Add(descriptor);
    }

    public static void UserInstallersFromAssemblyContaining<TAssemblyMarker>(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.UserInstallersFromAssemblyContaining(configuration, typeof(TAssemblyMarker));
    }

    public static void UserInstallersFromAssemblyContaining(this IServiceCollection services,
        IConfiguration configuration,
        params Type[] assemblyMarkers)
    {
        services.UserInstallersFromAssemblies(configuration,
            assemblyMarkers.Select(marker => marker.Assembly).ToArray());
    }

    private static void UserInstallersFromAssemblies(this IServiceCollection services, IConfiguration configuration,
        params Assembly[] assemblies)
    {
        var installers = assemblies.SelectMany(a => a.GetTypes())
            .Where(type => type.IsAssignableTo(typeof(IInstaller)) && type is { IsClass: true, IsAbstract: false })
            .Select(type => (IInstaller)Activator.CreateInstance(type)!)
            .Concat(assemblies.Select(assembly => new AnnotationInstaller(assembly)))
            .OrderBy(installer => installer.InstallOrder);

        foreach (var installer in installers)
        {
            installer.Install(services, configuration);
        }
    }
    
}