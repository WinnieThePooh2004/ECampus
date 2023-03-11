using ECampus.Core.Installers;
using ECampus.Domain.DataTransferObjects;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class UserServiceInstaller : IInstaller
{
    public int InstallOrder => 0;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBaseService<UserDto>, UserService>();
    }
}