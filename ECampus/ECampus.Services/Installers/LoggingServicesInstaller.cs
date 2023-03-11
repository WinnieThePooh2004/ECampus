using ECampus.Core.Installers;
using ECampus.Domain;
using ECampus.Domain.Extensions;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class LoggingServicesInstaller : IInstaller
{
    public int InstallOrder => int.MaxValue;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var dataTransferObjects = typeof(DomainAssemblyMarker).Assembly.GetDataTransferObjects();

        foreach (var dto in dataTransferObjects)
        {
            services.TryDecorate(typeof(IBaseService<>).MakeGenericType(dto),
                typeof(LoggingService<>).MakeGenericType(dto));
        }
    }
}