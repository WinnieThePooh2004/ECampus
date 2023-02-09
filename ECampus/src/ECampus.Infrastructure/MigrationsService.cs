﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ECampus.Infrastructure;

public class MigrationsService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public MigrationsService(IServiceProvider serviceProvider, ILogger logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>()!;
        try
        {
            await context.Database.MigrateAsync(stoppingToken);
        }

        catch (Exception e)
        {
            _logger.Error(e, "Unhandled exceptions occured while migrating database");
        }
    }
}