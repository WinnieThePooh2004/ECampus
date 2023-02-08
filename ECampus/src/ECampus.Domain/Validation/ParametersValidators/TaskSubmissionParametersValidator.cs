using System.Net;
using System.Security.Claims;
using ECampus.Contracts.DataAccess;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.Auth;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;
using ECampus.Contracts.DataSelectParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Domain.Validation.ParametersValidators;

public class TaskSubmissionParametersValidator : IParametersValidator<TaskSubmissionParameters>
{
    private readonly ClaimsPrincipal _user;
    private readonly IDataAccessManager _parametersDataAccess;

    public TaskSubmissionParametersValidator(IHttpContextAccessor httpContextAccessor,
        IDataAccessManager parametersDataAccess)
    {
        _parametersDataAccess = parametersDataAccess;
        _user = httpContextAccessor.HttpContext?.User ?? throw new HttpContextNotFoundExceptions();
    }

    public async Task<ValidationResult> ValidateAsync(TaskSubmissionParameters parameters, CancellationToken token = default)
    {
        var roleClaimValidation = _user.ValidateEnumClaim<UserRole>(ClaimTypes.Role);
        if (!roleClaimValidation.Result.IsValid)
        {
            return roleClaimValidation.Result;
        }

        return roleClaimValidation.ClaimValue switch
        {
            UserRole.Admin => new ValidationResult(),
            UserRole.Teacher => await ValidateAsTeacher(parameters, token),
            _ => throw new DomainException(HttpStatusCode.Forbidden,
                "You must be at least teacher to call this action"),
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
                new TeacherRelatedToTaskParameters { CourseTaskId = parameters.CourseTaskId });

        if (!await teachersRelatedToCourseTask.AnyAsync(teacher =>
                teacher.Id == teacherIdClaimValidation.ClaimValue &&
                teacher.Courses!.Any(course => course.Tasks!.Any()), cancellationToken: token))
        {
            return new ValidationResult(new ValidationError(nameof(parameters.CourseTaskId),
                "You are not teaching this course, so you can`t view its task"));
        }

        return new ValidationResult();
    }
}