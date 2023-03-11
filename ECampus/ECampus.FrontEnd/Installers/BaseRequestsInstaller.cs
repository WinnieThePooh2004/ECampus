using ECampus.Core.Installers;
using ECampus.FrontEnd.Requests;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared;
using ECampus.Shared.Extensions;

namespace ECampus.FrontEnd.Installers;

public class BaseRequestsInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var dataTransferObjects = typeof(SharedAssemblyMarker).Assembly.GetDataTransferObjects();

        foreach (var dataTransferObject in dataTransferObjects)
        {
            services.AddScoped(typeof(IBaseRequests<>).MakeGenericType(dataTransferObject), 
                typeof(BaseRequests<>).MakeGenericType(dataTransferObject));
        }
    }
}