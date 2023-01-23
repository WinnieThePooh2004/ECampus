using ECampus.Infrastructure.DataSelectors.SingleItemSelectors;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using ECampus.Shared.Installers;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Infrastructure.Installers;

public class SingleItemSelectorInstaller : IInstaller
{
    public int InstallOrder => 0;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var selectors = typeof(InfrastructureAssemblyMarker).Assembly.GetTypes().Where(type =>
            !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any() &&
            type.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISingleItemSelector<>))).ToList();

        var models = typeof(SharedAssemblyMarker).Assembly.GetModels();
        foreach (var modelType in models)
        {
            var modelSelector = selectors.SingleOrDefault(selector => selector.GetInterfaces()
                .Any(i => i == typeof(ISingleItemSelector<>).MakeGenericType(modelType)));
            if (modelSelector is null)
            {
                services.AddScoped(typeof(ISingleItemSelector<>).MakeGenericType(modelType),
                    typeof(SingleItemSelector<>).MakeGenericType(modelType));
                continue;
            }

            services.AddScoped(typeof(ISingleItemSelector<>).MakeGenericType(modelType), modelSelector);
        }
    }
}