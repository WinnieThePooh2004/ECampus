using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Core.Installers;

public interface IInstaller
{
    int InstallOrder { get; }

    void Install(IServiceCollection services, IConfiguration configuration);
}