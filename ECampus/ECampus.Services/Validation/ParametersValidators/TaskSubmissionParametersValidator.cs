using System.Net;
using System.Security.Claims;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.Auth;
using ECampus.Domain.Enums;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Validation.ParametersValidators;

public class TaskSubmissionParametersValidator : IParametersValidator<TaskSubmissionParameters>
{
    private readonly ClaimsPrincipal _user;
    private readonly IDataAccessFacade _parametersDataAccess;

    public TaskSubmissionParametersValidator(IHttpContextAccessor httpContextAccessor,
        IDataAccessFacade parametersDataAccess)
    {
        _parametersDataAccess = parametersDataAccess;
        _user = httpContextAccessor.HttpContext?.User ?? throw new HttpContextNotFoundExceptions();
    }

    public async Task<ValidationResult> ValidateAsync(TaskSubmissionParameters parameters, CancellationToken token = default)
    {
        var roleClaimValidation = _user.ValidateEnumClaim<UserRole>(ClaimTypes.Role);
        if (!roleClaimValidation.Result.IsValid)
        {
            throw new DomainException(HttpStatusCode.Unauthorized, "Role claim not found or it is not number");
        }

        return roleClaimValidation.ClaimValue switch
        {
            UserRole.Admin => new ValidationResult(),
            UserRole.Teacher => await ValidateAsTeacher(parameters, token),
            _ => throw new DomainException(HttpStatusCode.Forbidden,
                "You must be at least teacher to call this action")
        };
    }

    private async Task<ValidationResult> ValidateAsTeacher(TaskSubmissionParameters parameters, CancellationToken token = default)
    {
        var teacherIdClaimValidation = _user.ValidateParsableClaim<int>(CustomClaimTypes.TeacherId);
        if (!teacherIdClaimValidation.Result.IsValid)
        {
            return teacherIdClaimValidation.Result;
        }

        return await ValidateTeacher(parameters, teacherIdClaimValidation, token);
    }

    private async Task<ValidationResult> ValidateTeacher(TaskSubmissionParameters parameters,
        (ValidationResult Result, int? ClaimValue) teacherIdClaimValidation, CancellationToken token = default)
    {
        var teachersRelatedToCourseTask =
            _parametersDataAccess.GetByParameters<Teacher, TeacherRelatedToTaskParameters>(
                new TeacherRelatedToTaskParameters(parameters.CourseTaskId));

        if (!await teachersRelatedToCourseTask.AnyAsync(teacher =>
                teacher.Id == teacherIdClaimValidation.ClaimValue, cancellationToken: token))
        {
            return new ValidationResult(new ValidationError(nameof(parameters.CourseTaskId),
                "You are not teaching this course, so you can`t view its task"));
        }

        return new ValidationResult();
    }
}