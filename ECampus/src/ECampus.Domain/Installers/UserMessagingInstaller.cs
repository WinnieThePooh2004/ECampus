using ECampus.Domain.Messaging;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Installers;
using ECampus.Shared.Interfaces.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Domain.Installers;

public class UserMessagingInstaller : IInstaller
{
    public int InstallOrder => 3;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Decorate<IUserRolesService, UserRolesMessagingService>();
        services.Decorate<IBaseService<UserDto>, UserMessagingService>();
    }
}