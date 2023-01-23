﻿using ECampus.Domain.Services;
using ECampus.Shared;
using ECampus.Shared.Extensions;
using ECampus.Shared.Installers;
using ECampus.Shared.Interfaces.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Domain.Installers;

public class LoggingServicesInstaller : IInstaller
{
    public int InstallOrder => int.MaxValue;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var dataTransferObjects = typeof(SharedAssemblyMarker).Assembly.GetDataTransferObjects();

        foreach (var dto in dataTransferObjects)
        {
            services.Decorate(typeof(IBaseService<>).MakeGenericType(dto),
                typeof(LoggingService<>).MakeGenericType(dto));
        }
    }
}