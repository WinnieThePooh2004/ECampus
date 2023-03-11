using ECampus.Core.Installers;
using ECampus.Domain.DataTransferObjects;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Validation.UpdateValidators;
using ECampus.Validation;
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