using ECampus.Domain.Services;
using ECampus.Domain.Validation.CreateValidators;
using ECampus.Domain.Validation.FluentValidators;
using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Installers;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Domain.Validation;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Domain.Installers;

public class TaskSubmissionValidationInstaller : IInstaller
{
    public int InstallOrder => 3;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICreateValidator<TaskSubmissionDto>, TaskSubmissionCreateValidator>();
        services.AddScoped<IValidator<TaskSubmissionDto>, TaskSubmissionDtoValidator>();
        services.AddScoped<IUpdateValidator<TaskSubmissionDto>, UpdateFluentValidatorWrapper<TaskSubmissionDto>>();
        services.Decorate<IBaseService<TaskSubmissionDto>, ServiceWithCreateValidation<TaskSubmissionDto>>();
        services.Decorate<IBaseService<TaskSubmissionDto>, ServiceWithUpdateValidation<TaskSubmissionDto>>();
    }
}