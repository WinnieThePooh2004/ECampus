using ECampus.Infrastructure.DataCreateServices;
using ECampus.Infrastructure.DataDeleteServices;
using ECampus.Infrastructure.DataUpdateServices;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using ECampus.Shared.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Infrastructure.Installers;

public class DataServicesInstaller : IInstaller
{
    public int InstallOrder => -1;
    
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var models = typeof(SharedAssemblyMarker).Assembly.GetModels();

        foreach (var model in models)
        {
            services.AddScoped(typeof(IDataCreateService<>).MakeGenericType(model),
                typeof(DataCreateService<>).MakeGenericType(model));
            services.AddScoped(typeof(IDataDeleteService<>).MakeGenericType(model),
                typeof(DataDeleteService<>).MakeGenericType(model));
            services.AddScoped(typeof(IDataUpdateService<>).MakeGenericType(model),
                typeof(DataUpdateService<>).MakeGenericType(model));
        }
    }
}