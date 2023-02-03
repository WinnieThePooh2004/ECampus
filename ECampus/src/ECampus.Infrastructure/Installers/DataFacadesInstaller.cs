using ECampus.Contracts.DataAccess;
using ECampus.Core.Installers;
using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Infrastructure.Installers;

public class DataFacadesInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var models = typeof(SharedAssemblyMarker).Assembly.GetModels();

        foreach (var model in models)
        {
            services.AddScoped(typeof(IBaseDataAccessFacade<>).MakeGenericType(model),
                typeof(BaseDataAccessFacade<>).MakeGenericType(model));
        }
    }
}