using ECampus.Core.Installers;
using ECampus.Infrastructure.DataCreateServices;
using ECampus.Infrastructure.DataUpdateServices;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Infrastructure.Installers;

public class TaskSubmissionRelationsInstaller : IInstaller
{
    public int InstallOrder => 11;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Decorate<IDataCreateService<CourseTask>, CourseTaskCreateService>();
        services.Decorate<IDataCreateService<Student>, StudentCreateService>();
        services.Decorate<IDataUpdateService<Course>, CourseUpdateService>();
    }
}