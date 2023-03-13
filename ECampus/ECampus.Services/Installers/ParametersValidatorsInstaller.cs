using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.Domain.Requests;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Services.ValidationServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class ParametersValidatorsInstaller : IInstaller
{
    public int InstallOrder => 2;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var parametersValidators = typeof(ServicesAssemblyMarker).Assembly.GetTypes().Where(type =>
            type.GetInterfaces().Any(i => i.IsGenericOfType(typeof(IParametersValidator<>))));

        foreach (var parametersValidator in parametersValidators)
        {
            var parametersType = parametersValidator.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(IParametersValidator<>))).GetGenericArguments()[0];
            var parametersDto = parametersType.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(IQueryParameters<>))).GenericTypeArguments[0];
            
            services.AddScoped(typeof(IParametersValidator<>).MakeGenericType(parametersType),
                parametersValidator);
            services.Decorate(typeof(IParametersService<,>).MakeGenericType(parametersDto, parametersType),
                typeof(ServiceWithParametersValidation<,>).MakeGenericType(parametersDto, parametersType));
        }
    }
}