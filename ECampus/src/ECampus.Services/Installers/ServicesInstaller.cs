using ECampus.Contracts.Services;
using ECampus.Core.Installers;
using ECampus.Core.Metadata;
using ECampus.Services.Services;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using ECampus.Shared.Metadata;
using ECampus.Shared.QueryParameters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class ServicesInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var dataTransferObjects = typeof(SharedAssemblyMarker).Assembly.GetDataTransferObjects();

        foreach (var dataTransferObject in dataTransferObjects)
        {
            var metadata = dataTransferObject.GetCustomAttributes(typeof(DtoAttribute), false).OfType<DtoAttribute>()
                .Single();

            if (metadata.InjectBaseService)
            {
                services.AddScoped(typeof(IBaseService<>).MakeGenericType(dataTransferObject),
                    typeof(BaseService<,>).MakeGenericType(dataTransferObject, metadata.ModelType));
            }

            if (metadata.InjectParametersService)
            {
                InjectParametersFacadeIfExists(services, dataTransferObject, metadata.ModelType);
            }
        }
    }

    private static void InjectParametersFacadeIfExists(IServiceCollection services, Type dataTransferObject, Type model)
    {
        var modelParametersTypes = typeof(SharedAssemblyMarker).Assembly.GetTypes().Where(type =>
            !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any() &&
            type.IsAssignableTo(typeof(IQueryParameters<>).MakeGenericType(model)) &&
            type.IsAssignableTo(typeof(IQueryParameters)));

        foreach (var modelParametersType in modelParametersTypes)
        {
            services.AddScoped(typeof(IParametersService<,>).MakeGenericType(dataTransferObject, modelParametersType),
                typeof(ParametersService<,,>).MakeGenericType(dataTransferObject, modelParametersType, model));
        }
    }
}