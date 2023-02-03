using ECampus.Contracts.Services;
using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.Domain;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Services.Services;
using ECampus.Services.Services.ValidationServices;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using ECampus.Shared.Metadata;
using ECampus.Shared.QueryParameters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

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
            var validatorDtoTypes = dataTransferObjects.Where(dto =>
                dto.GetCustomAttributes(typeof(DtoAttribute), false).OfType<DtoAttribute>().Single().ModelType ==
                parametersModel);
            
            foreach (var validatorDtoType in validatorDtoTypes)
            {
                services.AddScoped(typeof(IParametersValidator<>).MakeGenericType(parametersType),
                    parametersValidator);
                services.Decorate(typeof(IParametersService<,>).MakeGenericType(validatorDtoType, parametersType),
                    typeof(ServiceWithParametersValidation<,>).MakeGenericType(validatorDtoType, parametersType));
            }
        }
    }
}