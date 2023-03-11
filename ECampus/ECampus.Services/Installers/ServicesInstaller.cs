using ECampus.Core.Installers;
using ECampus.Domain;
using ECampus.Domain.Extensions;
using ECampus.Domain.Metadata;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Services;
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
            var metadata = dataTransferObject.GetCustomAttributes(typeof(DtoAttribute), true).OfType<DtoAttribute>()
                .Single();

            if (metadata.InjectBaseService)
            {
                services.AddScoped(typeof(IBaseService<>).MakeGenericType(dataTransferObject),
                    typeof(BaseService<,>).MakeGenericType(dataTransferObject, metadata.ModelType));
            }
        }
    }
}