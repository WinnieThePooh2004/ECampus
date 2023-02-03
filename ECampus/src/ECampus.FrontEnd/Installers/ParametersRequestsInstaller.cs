using ECampus.Core.Installers;
using ECampus.FrontEnd.Requests;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared;
using ECampus.Shared.Extensions;
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
            var dtoParametersTypes = typeof(SharedAssemblyMarker).Assembly.GetTypes().Where(type =>
                type.IsAssignableTo(typeof(IQueryParameters<>).MakeGenericType(dataTransferObject
                    .GetCustomAttributes(typeof(DtoAttribute), false).OfType<DtoAttribute>().Single().ModelType)) &&
                type.IsAssignableTo(typeof(IQueryParameters)));

            foreach (var dtoParametersType in dtoParametersTypes)
            {
                services.AddScoped(
                    typeof(IParametersRequests<,>).MakeGenericType(dataTransferObject, dtoParametersType),
                    typeof(ParametersRequests<,>).MakeGenericType(dataTransferObject, dtoParametersType));
            }
        }
    }
}