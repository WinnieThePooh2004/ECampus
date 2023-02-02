using ECampus.Contracts.Services;
using ECampus.Core.Installers;
using ECampus.Services.Services;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class LoggingServicesInstaller : IInstaller
{
    public int InstallOrder => int.MaxValue;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var dataTransferObjects = typeof(SharedAssemblyMarker).Assembly.GetDataTransferObjects();

        foreach (var dto in dataTransferObjects)
        {
            services.TryDecorate(typeof(IBaseService<>).MakeGenericType(dto),
                typeof(LoggingService<>).MakeGenericType(dto));
        }
    }
}