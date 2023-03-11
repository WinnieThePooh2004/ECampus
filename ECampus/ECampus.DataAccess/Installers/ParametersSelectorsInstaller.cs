using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.DataAccess.Installers;

public class ParametersSelectorsInstaller : IInstaller
{
    public int InstallOrder => 0;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var selectors = typeof(DataAccessAssemblyMarker).Assembly.GetTypes()
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericOfType(typeof(IParametersSelector<,>))) &&
                           type is { IsAbstract: false, IsClass: true, IsGenericType: false });
        foreach (var selector in selectors)
        {
            var selectorParameters = selector.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(IParametersSelector<,>))).GetGenericArguments();
            services.AddScoped(typeof(IParametersSelector<,>).MakeGenericType(selectorParameters), selector);
        }
    }
}