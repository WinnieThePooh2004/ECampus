using System.Net;
using System.Security.Claims;
using ECampus.Contracts.DataValidation;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.Auth;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Domain.Validation.ParametersValidators;

public class TaskSubmissionParametersValidator : IParametersValidator<TaskSubmissionParameters>
{
    private readonly ClaimsPrincipal _user;
    private readonly IParametersDataValidator<TaskSubmissionParameters> _parametersDataValidator;

    public TaskSubmissionParametersValidator(IHttpContextAccessor httpContextAccessor,
        IParametersDataValidator<TaskSubmissionParameters> parametersDataValidator)
    {
        _parametersDataValidator = parametersDataValidator;
        _user = httpContextAccessor.HttpContext?.User ?? throw new HttpContextNotFoundExceptions();
    }

    public async Task<ValidationResult> ValidateAsync(TaskSubmissionParameters parameters)
    {
        var roleAsString = _user.FindFirst(ClaimTypes.Role)?.Value ??
                           throw new DomainException(HttpStatusCode.Unauthorized,
                               "Role claim not found");
        if (!Enum.TryParse<UserRole>(roleAsString, out var currentUserRole))
        {
            throw new DomainException(HttpStatusCode.Forbidden, $"No such role '{roleAsString}'");
        }

        return currentUserRole switch
        {
            UserRole.Admin => new ValidationResult(),
            UserRole.Teacher => await ValidateAsTeacher(parameters),
            _ => throw new DomainException(HttpStatusCode.Forbidden,
                "You must be at least teacher to perform this action"),
        };
    }

    private async Task<ValidationResult> ValidateAsTeacher(TaskSubmissionParameters parameters)
    {
        var teacherIdClaimValidation = _user.ValidateParsableClaim<int>(CustomClaimTypes.TeacherId);
        if (!teacherIdClaimValidation.Result.IsValid)
        {
            return teacherIdClaimValidation.Result;
        }

        return await _parametersDataValidator.ValidateAsync(parameters);
    }
}