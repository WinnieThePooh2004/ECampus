using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.DataAccess.Installers;

public class MultipleItemSelectorsInstaller : IInstaller
{
    public int InstallOrder => 0;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var selectors = typeof(DataAccessAssemblyMarker).Assembly.GetTypes()
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericOfType(typeof(IMultipleItemSelector<,>))) &&
                           !type.GetCustomAttributes(typeof(DataAccessAssemblyMarker), false).Any());
        foreach (var selector in selectors)
        {
            var selectorParameters = selector.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(IMultipleItemSelector<,>))).GetGenericArguments();
            services.AddScoped(typeof(IMultipleItemSelector<,>).MakeGenericType(selectorParameters), selector);
        }
    }
}