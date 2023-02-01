using ECampus.Core.Extensions;
using ECampus.Core.Installers;
using ECampus.Core.Metadata;
using ECampus.Domain.Interfaces.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Domain.Installers;

public class UniqueValidatorsInstaller : IInstaller
{
    public int InstallOrder => 4;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        InstallUpdateValidators(services);
        InstallCreateValidators(services);
    }

    private static void InstallCreateValidators(IServiceCollection services)
    {
        var createValidators = typeof(DomainAssemblyMarker).Assembly.GetTypes()
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericOfType(typeof(ICreateValidator<>))) &&
                           type is { IsAbstract: false, IsClass: true, IsGenericType: false } &&
                           !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any());

        foreach (var createValidator in createValidators)
        {
            var validationType = createValidator.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(ICreateValidator<>)))
                .GetGenericArguments()[0];

            services.Decorate(typeof(ICreateValidator<>).MakeGenericType(validationType), createValidator);
        }
    }

    private static void InstallUpdateValidators(IServiceCollection services)
    {
        var updateValidators = typeof(DomainAssemblyMarker).Assembly.GetTypes()
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericOfType(typeof(IUpdateValidator<>))) &&
                           type is { IsAbstract: false, IsClass: true, IsGenericType: false });

        foreach (var updateValidator in updateValidators)
        {
            var validationType = updateValidator.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(IUpdateValidator<>)))
                .GetGenericArguments()[0];

            services.Decorate(typeof(IUpdateValidator<>).MakeGenericType(validationType), updateValidator);
        }
    }
}