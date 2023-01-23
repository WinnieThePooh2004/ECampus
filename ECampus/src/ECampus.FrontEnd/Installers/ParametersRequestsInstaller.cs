using ECampus.FrontEnd.Requests;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using ECampus.Shared.Installers;
using ECampus.Shared.Metadata;
using ECampus.Shared.QueryParameters;

namespace ECampus.FrontEnd.Installers;

public class ParametersRequestsInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var dataTransferObjects = typeof(SharedAssemblyMarker).Assembly.GetDataTransferObjects();
        foreach (var dataTransferObject in dataTransferObjects)
        {
            var typeParameters = typeof(SharedAssemblyMarker).Assembly.GetTypes().SingleOrDefault(type =>
                type.IsAssignableTo(typeof(IQueryParameters<>).MakeGenericType(dataTransferObject
                    .GetCustomAttributes(typeof(DtoAttribute), false).OfType<DtoAttribute>().Single().ModelType)));

            if (typeParameters is null)
            {
                continue;
            }

            services.AddScoped(typeof(IParametersRequests<,>).MakeGenericType(dataTransferObject, typeParameters),
                typeof(ParametersRequests<,>).MakeGenericType(dataTransferObject, typeParameters));
        }
    }
}