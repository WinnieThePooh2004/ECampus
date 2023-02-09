using ECampus.Core.Installers;
using ECampus.DataAccess.DataCreateServices;
using ECampus.DataAccess.DataUpdateServices;
using ECampus.DataAccess.Interfaces;
using ECampus.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.DataAccess.Installers;

public class TaskSubmissionRelationsInstaller : IInstaller
{
    public int InstallOrder => 11;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Decorate<IDataCreateService<Student>, StudentCreateService>();
        services.Decorate<IDataUpdateService<Course>, CourseUpdateService>();
    }
}