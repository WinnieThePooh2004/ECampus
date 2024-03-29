﻿using ECampus.Core.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace ECampus.WebApi.Installers;

public class LoggerInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console()
            .WriteTo.MSSqlServer(
                configuration.GetConnectionString("ApplicationDbContext"),
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