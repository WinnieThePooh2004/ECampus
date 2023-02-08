using ECampus.Contracts.DataAccess;
using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Infrastructure.Installers;

public class MultipleItemSelectorsInstaller : IInstaller
{
    public int InstallOrder => 0;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var selectors = typeof(InfrastructureAssemblyMarker).Assembly.GetTypes()
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericOfType(typeof(IMultipleItemSelector<,>))) &&
                           !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any());
        foreach (var selector in selectors)
        {
            var selectorParameters = selector.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(IMultipleItemSelector<,>))).GetGenericArguments();
            services.AddScoped(typeof(IMultipleItemSelector<,>).MakeGenericType(selectorParameters), selector);
        }
    }
}