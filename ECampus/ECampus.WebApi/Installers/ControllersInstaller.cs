using ECampus.Core.Installers;
using ECampus.WebApi.MiddlewareFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ECampus.WebApi.Installers;

public class ControllersInstaller : IInstaller
{
    public int InstallOrder => -1;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options => { options.Filters.Add<MiddlewareExceptionFilter>(); })
            .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

    }
}