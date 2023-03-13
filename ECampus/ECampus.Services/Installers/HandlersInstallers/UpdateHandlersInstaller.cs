using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.Domain;
using ECampus.Domain.Commands;
using ECampus.Services.Handlers.UpdateHandlers;
using ECampus.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers.HandlersInstallers;

public class UpdateHandlersInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var updateCommandTypes = typeof(DomainAssemblyMarker).Assembly
            .GetTypes().Where(type => type.IsAssignableTo(typeof(IUpdateCommand)) &&
                                      type is { IsAbstract: false, IsClass: true } &&
                                      !type.GetCustomAttributes(true).OfType<InstallerIgnoreAttribute>().Any());

        foreach (var updateCommandType in updateCommandTypes)
        {
            var entityType = updateCommandType.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(IUpdateCommand<>))).GetGenericArguments()[0];

            services.AddScoped(typeof(IUpdateHandler<>).MakeGenericType(updateCommandType),
                typeof(UpdateHandler<,>).MakeGenericType(updateCommandType, entityType));
        }
    }
}