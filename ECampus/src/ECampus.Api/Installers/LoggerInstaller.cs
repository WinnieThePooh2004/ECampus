using ECampus.Core.Installers;
using ECampus.Infrastructure;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace ECampus.Api.Installers;

public class LoggerInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console()
            .WriteTo.MSSqlServer(
                configuration.GetConnectionString(ApplicationDbContext.ConnectionKey),
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    SchemaName = "dbo",
                    AutoCreateSqlTable = true
                },
                restrictedToMinimumLevel: Enum.Parse<LogEventLevel>(configuration["Logging:LogLevel:Default"] ??
                                                                    throw new Exception(
                                                                        "Cannot find 'Logging:LogLevel:Default'"))
            ).CreateLogger();

        services.AddSingleton(Log.Logger);
    }
}