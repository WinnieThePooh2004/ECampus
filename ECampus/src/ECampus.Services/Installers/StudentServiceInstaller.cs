﻿using ECampus.Contracts.Services;
using ECampus.Core.Installers;
using ECampus.Services.Services;
using ECampus.Shared.DataTransferObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class StudentServiceInstaller : IInstaller
{
    public int InstallOrder => 0;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Decorate<IBaseService<StudentDto>, StudentService>();
    }
}