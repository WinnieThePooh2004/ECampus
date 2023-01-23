using ECampus.Shared.Extensions;
using ECampus.Shared.Installers;
using ECampus.Shared.Interfaces.Data.DataServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Infrastructure.Installers;

public class MultipleItemSelectorsInstaller : IInstaller
{
    public int InstallOrder => 0;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var selectors = typeof(InfrastructureAssemblyMarker).Assembly.GetTypes()
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericOfType(typeof(IMultipleItemSelector<,>))));
        foreach (var selector in selectors)
        {
            var selectorParameters = selector.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(IMultipleItemSelector<,>))).GetGenericArguments();
            services.AddScoped(typeof(IMultipleItemSelector<,>).MakeGenericType(selectorParameters), selector);
        }
    }
}