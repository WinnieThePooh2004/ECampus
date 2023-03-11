using ECampus.Core.Installers;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.DataAccess.DataSelectors.MultipleItemSelectors;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain;
using ECampus.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.DataAccess.Installers;

public class PureIdSelectInstallers : IInstaller
{
    public int InstallOrder => -1;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var models = typeof(DomainAssemblyMarker).Assembly.GetModels();
        foreach (var model in models)
        {
            var parametersType = typeof(PureByIdParameters<>).MakeGenericType(model);
            services.AddSingleton(typeof(IParametersSelector<,>).MakeGenericType(model, parametersType),
                typeof(PureObjectByIdSelect<>).MakeGenericType(model));
        }
    }
}