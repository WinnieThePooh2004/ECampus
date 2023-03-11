using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.FrontEnd.Requests;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Domain;
using ECampus.Domain.QueryParameters;

namespace ECampus.FrontEnd.Installers;

public class ParametersRequestsInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var parametersTypes = typeof(SharedAssemblyMarker)
            .Assembly.GetTypes().Where(type => type.IsAssignableTo(typeof(IQueryParameters)) &&
                                               type is {IsAbstract: false, IsClass: true});

        foreach (var parametersType in parametersTypes)
        {
            var dtoType = parametersType.GetInterfaces().Single(i => i.IsGenericOfType(typeof(IQueryParameters<>)))
                .GenericTypeArguments[0];
            services.AddScoped(typeof(IParametersRequests<,>).MakeGenericType(dtoType, parametersType),
                typeof(ParametersRequests<,>).MakeGenericType(dtoType, parametersType));
        }
    }
}