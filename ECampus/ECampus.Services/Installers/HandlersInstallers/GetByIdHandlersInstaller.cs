using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.Domain;
using ECampus.Domain.Responses;
using ECampus.Services.Handlers.GetByIdHandlers;
using ECampus.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers.HandlersInstallers;

public class GetByIdHandlersInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var getByIdResponseTypes = typeof(DomainAssemblyMarker).Assembly
            .GetTypes().Where(type => type.IsAssignableTo(typeof(ISingleItemResponse)) &&
                                      type is { IsAbstract: false, IsClass: true } &&
                                      !type.GetCustomAttributes(true).OfType<InstallerIgnoreAttribute>().Any());

        foreach (var getByIdResponseType in getByIdResponseTypes)
        {
            var entityType = getByIdResponseType.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(ISingleItemResponse<>))).GetGenericArguments()[0];

            services.AddScoped(typeof(IGetByIdHandler<>).MakeGenericType(getByIdResponseType),
                typeof(GetByIdHandler<,>).MakeGenericType(getByIdResponseType, entityType));
        }
    }
}