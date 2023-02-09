using ECampus.Contracts.DataSelectParameters;
using ECampus.Core.Installers;
using ECampus.DataAccess.DataSelectors.MultipleItemSelectors;
using ECampus.DataAccess.Interfaces;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.DataAccess.Installers;

public class PureIdSelectInstallers : IInstaller
{
    public int InstallOrder => -1;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var models = typeof(SharedAssemblyMarker).Assembly.GetModels();
        foreach (var model in models)
        {
            var parametersType = typeof(PureByIdParameters<>).MakeGenericType(model);
            services.AddSingleton(typeof(IParametersSelector<,>).MakeGenericType(model, parametersType),
                typeof(PureObjectByIdSelect<>).MakeGenericType(model));
        }
    }
}