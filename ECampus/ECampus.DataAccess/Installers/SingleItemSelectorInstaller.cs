using ECampus.Core.Installers;
using ECampus.DataAccess.DataSelectors.SingleItemSelectors;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain;
using ECampus.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.DataAccess.Installers;

public class SingleItemSelectorInstaller : IInstaller
{
    public int InstallOrder => 0;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var selectors = typeof(DataAccessAssemblyMarker).Assembly.GetTypes().Where(type =>
            !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any() &&
            type.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISingleItemSelector<>))).ToList();

        var models = typeof(DomainAssemblyMarker).Assembly.GetModels();
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