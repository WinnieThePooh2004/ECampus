using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Services;
using ECampus.Shared;
using ECampus.Shared.QueryParameters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class ParametersServicesInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var allParameters = typeof(SharedAssemblyMarker).Assembly.GetTypes().Where(type =>
            type is { IsAbstract: false, IsClass: true } && type.IsAssignableTo(typeof(IQueryParameters)));

        foreach (var parameters in allParameters)
        {
            var modelType = parameters.GetInterfaces().Single(i => i.IsGenericOfType(typeof(IDataSelectParameters<>)))
                .GetGenericArguments()[0];
            var dtoType = parameters.GetInterfaces().Single(i => i.IsGenericOfType(typeof(IQueryParameters<>)))
                .GetGenericArguments()[0];

            services.AddScoped(typeof(IParametersService<,>).MakeGenericType(dtoType, parameters),
                typeof(ParametersService<,,>).MakeGenericType(dtoType, parameters, modelType));
        }
    }
}