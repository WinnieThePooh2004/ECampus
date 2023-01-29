using ECampus.Infrastructure.DataCreateServices;
using ECampus.Infrastructure.DataUpdateServices;
using ECampus.Shared.Installers;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Infrastructure.Installers;

public class TaskSubmissionRelationsInstaller : IInstaller
{
    public int InstallOrder => 11;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Decorate<IDataCreateService<Student>, StudentCreateService>();
        services.Decorate<IDataUpdateService<Course>, CourseUpdateService>();
    }
}