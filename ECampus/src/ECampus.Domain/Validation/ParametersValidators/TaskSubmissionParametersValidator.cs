using System.Net;
using System.Security.Claims;
using ECampus.Shared.Auth;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Domain.Validation.ParametersValidators;

public class TaskSubmissionParametersValidator : IParametersValidator<TaskSubmissionParameters>
{
    private readonly ClaimsPrincipal _user;

    public TaskSubmissionParametersValidator(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext?.User ?? throw new HttpContextNotFoundExceptions();
    }

    public Task<ValidationResult> Validate(TaskSubmissionParameters parameters)
    {
        var roleAsString = _user.FindFirst(ClaimTypes.Role)?.Value ?? throw new DomainException((HttpStatusCode)401);
        if (!Enum.TryParse<UserRole>(roleAsString, out var currentUserRole))
        {
            throw new DomainException(HttpStatusCode.Forbidden, $"No such role '{roleAsString}'");
        }

        return currentUserRole switch
        {
            UserRole.Guest => throw new DomainException(HttpStatusCode.Forbidden,
                "You must be at least student to perform this action"),
            UserRole.Admin or UserRole.Teacher => Task.FromResult(new ValidationResult()),
            _ => Task.FromResult(ValidateAsStudent(parameters, currentUserRole))
        };
    }

    private ValidationResult ValidateAsStudent(TaskSubmissionParameters parameters, UserRole currentUserRole)
    {
        var currentStudentId = _user.FindFirst(CustomClaimTypes.StudentId)?.Value ??
                               throw new DomainException(HttpStatusCode.Forbidden, "If you are logged in as" +
                                   " student you must have StudentId claim");

        if (currentUserRole == UserRole.Student && parameters.StudentId.ToString() != currentStudentId)
        {
            return new ValidationResult(new ValidationError(nameof(parameters.StudentId),
                $"You cannot request for submissions with student id than is not equal to yours"));
        }

        return new ValidationResult();
    }
}