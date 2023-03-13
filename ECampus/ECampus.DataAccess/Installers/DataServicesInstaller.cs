using ECampus.Core.Installers;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.DataAccessFacades;
using ECampus.DataAccess.DataCreateServices;
using ECampus.DataAccess.DataDeleteServices;
using ECampus.DataAccess.DataUpdateServices;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain;
using ECampus.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.DataAccess.Installers;

public class DataServicesInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var models = typeof(DomainAssemblyMarker).Assembly.GetEntities();

        foreach (var model in models)
        {
            services.AddScoped(typeof(IDataCreateService<>).MakeGenericType(model),
                typeof(DataCreateService<>).MakeGenericType(model));
            services.AddScoped(typeof(IDataDeleteService<>).MakeGenericType(model),
                typeof(DataDeleteService<>).MakeGenericType(model));
            services.AddScoped(typeof(IDataUpdateService<>).MakeGenericType(model),
                typeof(DataUpdateService<>).MakeGenericType(model));
        }

        services.AddScoped<IDataAccessFacade, DataAccessFacade>();
    }
}