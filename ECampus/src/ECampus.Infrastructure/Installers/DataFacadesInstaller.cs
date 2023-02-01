using ECampus.Contracts.DataAccess;
using ECampus.Core.Installers;
using ECampus.Core.Metadata;
using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using ECampus.Shared.QueryParameters;
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

            InjectParametersFacadeIfExists(services, model);
        }
    }

    private static void InjectParametersFacadeIfExists(IServiceCollection services, Type model)
    {
        var modelParameters = typeof(SharedAssemblyMarker).Assembly.GetTypes().SingleOrDefault(type =>
            type.IsAssignableTo(typeof(IQueryParameters<>).MakeGenericType(model)) &&
            !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any());

        if (modelParameters is null)
        {
            return;
        }

        services.AddScoped(typeof(IParametersDataAccessFacade<,>).MakeGenericType(model, modelParameters),
            typeof(ParametersDataAccessFacade<,>).MakeGenericType(model, modelParameters));
    }
}