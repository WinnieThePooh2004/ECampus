using ECampus.Core.Installers;

namespace ECampus.Api.Installers;

public class CorsInstaller : IInstaller
{
    public int InstallOrder => -1;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: configuration["Cors:Name"] ?? throw new Exception("cannot find cors name"),
                policy =>
                {
                    policy.WithOrigins(configuration
                                           .GetSection("Cors:Origins").Get<string[]>()?.ToArray() ??
                                       throw new Exception("cannot find cors origins"));
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
        });
    }
}