using ECampus.Core.Installers;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Validation.FluentValidators;
using ECampus.Services.Validation.UpdateValidators;
using ECampus.Shared.DataTransferObjects;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Services.Installers;

public class TaskSubmissionValidationInstaller : IInstaller
{
    public int InstallOrder => 3;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IValidator<TaskSubmissionDto>, TaskSubmissionDtoValidator>();
        services.AddScoped<IUpdateValidator<TaskSubmissionDto>, UpdateFluentValidatorWrapper<TaskSubmissionDto>>();
    }
}