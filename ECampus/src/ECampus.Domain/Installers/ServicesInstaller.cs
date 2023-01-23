using ECampus.Domain.Services;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using ECampus.Shared.Installers;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Metadata;
using ECampus.Shared.QueryParameters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Domain.Installers;

public class ServicesInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var dataTransferObjects = typeof(SharedAssemblyMarker).Assembly.GetDataTransferObjects();

        foreach (var dataTransferObject in dataTransferObjects)
        {
            var model = dataTransferObject.GetCustomAttributes(typeof(DtoAttribute), false).OfType<DtoAttribute>()
                .Single().ModelType;

            services.AddScoped(typeof(IBaseService<>).MakeGenericType(dataTransferObject), 
                typeof(BaseService<,>).MakeGenericType(dataTransferObject, model));
            
            InjectParametersFacadeIfExists(services, dataTransferObject, model);
        }
    }
    
    private static void InjectParametersFacadeIfExists(IServiceCollection services, Type dataTransferObject, Type model)
    {
        var modelParameters = typeof(SharedAssemblyMarker).Assembly.GetTypes().SingleOrDefault(type =>
            !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any() &&
            type.IsAssignableTo(typeof(IQueryParameters<>).MakeGenericType(model)));

        if (modelParameters is null)
        {
            return;
        }

        services.AddScoped(typeof(IParametersService<,>).MakeGenericType(dataTransferObject, modelParameters),
            typeof(ParametersService<,,>).MakeGenericType(dataTransferObject, modelParameters, model));
    }
}