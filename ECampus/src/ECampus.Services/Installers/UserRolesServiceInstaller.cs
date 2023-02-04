using ECampus.Core.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class UserRolesServiceInstaller : IInstaller
{
    public int InstallOrder => 0;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddScoped<IUserro>();
    }
}