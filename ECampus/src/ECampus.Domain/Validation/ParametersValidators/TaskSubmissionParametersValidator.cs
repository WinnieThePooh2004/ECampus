using System.Net;
using System.Security.Claims;
using ECampus.Shared.Auth;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.DataAccess.Validation;
using ECampus.Shared.Interfaces.Domain.Validation;
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

    public async Task<ValidationResult> Validate(TaskSubmissionParameters parameters)
    {
        var roleAsString = _user.FindFirst(ClaimTypes.Role)?.Value ??
                           throw new DomainException(HttpStatusCode.Forbidden,
                               "You must be at least student to perform this action");
        if (!Enum.TryParse<UserRole>(roleAsString, out var currentUserRole))
        {
            throw new DomainException(HttpStatusCode.Forbidden, $"No such role '{roleAsString}'");
        }

        return currentUserRole switch
        {
            UserRole.Admin => new ValidationResult(),
            UserRole.Teacher => await ValidateAsTeacher(parameters),
            UserRole.Student or UserRole.Guest => throw new DomainException(HttpStatusCode.Forbidden,
                "You must be at least student to perform this action"),
            _ => throw new ArgumentOutOfRangeException(nameof(parameters))
        };
    }

    private async Task<ValidationResult> ValidateAsTeacher(TaskSubmissionParameters parameters)
    {
        var teacherIdClaim = _user.FindFirst(CustomClaimTypes.TeacherId);
        if (teacherIdClaim is null || !int.TryParse(teacherIdClaim.Value, out _))
        {
            return new ValidationResult(new ValidationError(nameof(teacherIdClaim), 
                "Yor are now registered as teacher or your TeacherId claim is not a number"));
        }
        return await _parametersDataValidator.ValidateAsync(parameters);
    }
}