using ECampus.Core.Installers;
using ECampus.Domain;
using ECampus.Domain.Extensions;
using ECampus.Services.Handlers.DeleteHandlers;
using ECampus.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers.HandlersInstallers;

public class DeleteHandlersInstaller : IInstaller
{
    public int InstallOrder => -1;
    
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var entities = typeof(DomainAssemblyMarker).Assembly.GetEntities();

        foreach (var entityType in entities)
        {
            services.AddScoped(typeof(IDeleteHandler<>).MakeGenericType(entityType), 
                typeof(DeleteHandler<>).MakeGenericType(entityType));
        }
    }
}