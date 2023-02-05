using ECampus.Contracts.Services;
using ECampus.Core.Installers;
using ECampus.Domain.Messaging;
using ECampus.Shared.DataTransferObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Domain.Installers;

public class MessagingInstaller : IInstaller
{
    public int InstallOrder => 3;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Decorate<IBaseService<UserDto>, UserMessagingService>();
        services.Decorate<IPasswordChangeService, PasswordChangeMessagingService>();
        services.Decorate<IBaseService<CourseTaskDto>, CourseTaskMessagingService>();
        services.Decorate<ITaskSubmissionService, TaskSubmissionMessagingService>();
    }
}