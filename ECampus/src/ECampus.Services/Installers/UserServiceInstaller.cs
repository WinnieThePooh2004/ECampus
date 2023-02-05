using ECampus.Contracts.Services;
using ECampus.Core.Installers;
using ECampus.Services.Services;
using ECampus.Shared.DataTransferObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class UserServiceInstaller : IInstaller
{
    public int InstallOrder => 0;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<UserRolesService>();
        services.Decorate<IBaseService<UserDto>, UserBaseService>();
    }
}