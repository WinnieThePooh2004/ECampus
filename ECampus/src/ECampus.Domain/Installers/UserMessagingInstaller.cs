using ECampus.Contracts.Services;
using ECampus.Core.Installers;
using ECampus.Domain.Messaging;
using ECampus.Shared.DataTransferObjects;
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