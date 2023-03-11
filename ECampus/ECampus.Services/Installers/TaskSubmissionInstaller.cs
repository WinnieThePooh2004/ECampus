using ECampus.Core.Installers;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Services;
using ECampus.Services.Services.ValidationServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class TaskSubmissionInstaller : IInstaller
{
    public int InstallOrder => -1;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITaskSubmissionService, TaskSubmissionService>();
        services.Decorate<ITaskSubmissionService, TaskSubmissionValidationService>();
    }
}