using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Validation.CreateValidators;
using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Shared.Metadata;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Domain.Installers;

public class FluentValidationWrapperInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var fluentValidators = typeof(DomainAssemblyMarker).Assembly.GetTypes()
            .Where(type => type.BaseType is not null && type.BaseType.IsGenericOfType(typeof(AbstractValidator<>)) &&
                           !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any());

        foreach (var fluentValidator in fluentValidators)
        {
            AddFluentValidator(services, fluentValidator);
        }
    }

    private static void AddFluentValidator(IServiceCollection services, Type fluentValidator)
    {
        var validatingType = fluentValidator.BaseType!.GenericTypeArguments[0];
        var typeValidation = validatingType.GetCustomAttributes(typeof(ValidationAttribute), false)
            .OfType<ValidationAttribute>().SingleOrDefault();
        if (typeValidation is null)
        {
            return;
        }
        services.AddScoped(typeof(IValidator<>).MakeGenericType(validatingType), fluentValidator);

        if (typeValidation.ValidateCreate)
        {
            services.AddScoped(typeof(ICreateValidator<>).MakeGenericType(validatingType),
                typeof(CreateFluentValidatorWrapper<>).MakeGenericType(validatingType));
        }

        if (typeValidation.ValidateUpdate)
        {
            services.AddScoped(typeof(IUpdateValidator<>).MakeGenericType(validatingType),
                typeof(UpdateFluentValidatorWrapper<>).MakeGenericType(validatingType));
        }
    }
}