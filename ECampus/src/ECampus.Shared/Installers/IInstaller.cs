using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Shared.Installers;

public interface IInstaller
{
    int InstallOrder { get; }

    void Install(IServiceCollection services, IConfiguration configuration);
}