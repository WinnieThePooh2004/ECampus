﻿using ECampus.Core.Installers;
using ECampus.Domain;
using ECampus.Domain.Metadata;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Services.ValidationServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class ValidationServicesInstaller : IInstaller
{
    public int InstallOrder => 10;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var typesWithValidation = typeof(DomainAssemblyMarker).Assembly.GetTypes()
            .Where(type => type.GetCustomAttributes(typeof(ValidationAttribute), false).Any() &&
                           !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any());

        foreach (var type in typesWithValidation)
        {
            var typeValidation =
                (ValidationAttribute)type.GetCustomAttributes(typeof(ValidationAttribute), false).Single();

            if (!typeValidation.DecorateServices)
            {
                continue;
            }

            if (typeValidation.ValidateCreate)
            {
                services.Decorate(typeof(IBaseService<>).MakeGenericType(type),
                    typeof(ServiceWithCreateValidation<>).MakeGenericType(type));
            }

            if (typeValidation.ValidateUpdate)
            {
                services.Decorate(typeof(IBaseService<>).MakeGenericType(type),
                    typeof(ServiceWithUpdateValidation<>).MakeGenericType(type));
            }
        }
    }
}