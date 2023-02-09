using ECampus.Contracts.DataAccess;
using ECampus.Core.Installers;
using ECampus.DataAccess.DataAccessFacades;
using ECampus.DataAccess.DataCreateServices;
using ECampus.DataAccess.DataDeleteServices;
using ECampus.DataAccess.DataUpdateServices;
using ECampus.DataAccess.Interfaces;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.DataAccess.Installers;

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

        services.AddScoped<IDataAccessManager, DataAccessManager>();
    }
}