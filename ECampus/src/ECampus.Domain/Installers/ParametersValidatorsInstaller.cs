using ECampus.Domain.Services;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using ECampus.Shared.Installers;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Metadata;
using ECampus.Shared.QueryParameters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Domain.Installers;

public class ParametersValidatorsInstaller : IInstaller
{
    public int InstallOrder => 2;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var parametersValidators = typeof(DomainAssemblyMarker).Assembly.GetTypes().Where(type =>
            type.GetInterfaces().Any(i => i.IsGenericOfType(typeof(IParametersValidator<>))));

        var dataTransferObjects = typeof(SharedAssemblyMarker).Assembly.GetDataTransferObjects().ToList();

        foreach (var parametersValidator in parametersValidators)
        {
            var parametersType = parametersValidator.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(IParametersValidator<>))).GetGenericArguments()[0];
            var parametersModel = parametersType.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(IQueryParameters<>))).GenericTypeArguments[0];
            var validatorDto = dataTransferObjects.Single(dto =>
                dto.GetCustomAttributes(typeof(DtoAttribute), false).OfType<DtoAttribute>().Single().ModelType ==
                parametersModel);

            services.AddScoped(typeof(IParametersValidator<>).MakeGenericType(parametersType),
                parametersValidator);
            services.Decorate(typeof(IParametersService<,>).MakeGenericType(validatorDto, parametersType),
                typeof(ServiceWithParametersValidation<,>).MakeGenericType(validatorDto, parametersType));
        }
    }
}