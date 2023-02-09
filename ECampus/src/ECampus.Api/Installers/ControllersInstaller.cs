using ECampus.Api.MiddlewareFilters;
using ECampus.Core.Installers;
using Newtonsoft.Json;

namespace ECampus.Api.Installers;

public class ControllersInstaller : IInstaller
{
    public int InstallOrder => -1;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options => { options.Filters.Add<MiddlewareExceptionFilter>(); })
            .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

    }
}