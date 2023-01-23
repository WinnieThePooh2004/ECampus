using ECampus.FrontEnd.Requests.Options;
using ECampus.Shared.Installers;

namespace ECampus.FrontEnd.Installers;

public class RequestsOptionsInstaller : IInstaller
{
    public int InstallOrder => -1;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRequestOptions>(new RequestOptions(configuration));
    }
}