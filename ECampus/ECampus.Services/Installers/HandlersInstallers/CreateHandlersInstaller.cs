using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.Domain;
using ECampus.Domain.Commands;
using ECampus.Services.Handlers.CreateHandlers;
using ECampus.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers.HandlersInstallers;

public class CreateHandlersInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var createCommandTypes = typeof(DomainAssemblyMarker).Assembly
            .GetTypes().Where(type => type.IsAssignableTo(typeof(ICreateCommand)) &&
                                      type is { IsAbstract: false, IsClass: true } &&
                !type.GetCustomAttributes(true).OfType<InstallerIgnoreAttribute>().Any());

        foreach (var createCommandType in createCommandTypes)
        {
            var entityType = createCommandType.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(ICreateCommand<>))).GetGenericArguments()[0];

            services.AddScoped(typeof(ICreateHandler<>).MakeGenericType(createCommandType),
                typeof(CreateHandler<,>).MakeGenericType(createCommandType, entityType));
        }
    }
}