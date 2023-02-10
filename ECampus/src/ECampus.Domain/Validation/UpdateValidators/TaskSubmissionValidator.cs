using System.Security.Claims;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Core.Installers;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Domain.Validation.UpdateValidators;

[Inject(typeof(ITaskSubmissionValidator))]
public class TaskSubmissionValidator : ITaskSubmissionValidator
{
    private readonly ClaimsPrincipal _user;
    private readonly IValidator<TaskSubmissionDto> _validator;
    private readonly IDataAccessManager _parametersDataAccess;

    public TaskSubmissionValidator(IHttpContextAccessor httpContextAccessor, IValidator<TaskSubmissionDto> validator,
        IDataAccessManager parametersDataAccess)
    {
        _user = httpContextAccessor.HttpContext?.User ?? throw new HttpContextNotFoundExceptions();
        _validator = validator;
        _parametersDataAccess = parametersDataAccess;
    }

    public async Task<ValidationResult> ValidateUpdateContentAsync(int submissionId, string content)
    {
        var errorsFromFluentValidator =
            await _validator.ValidateAsync(new TaskSubmissionDto { SubmissionContent = content });
        if (!errorsFromFluentValidator.IsValid)
        {
            return new ValidationResult(
                errorsFromFluentValidator.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage)));
        }

        return await ValidateStudentId(submissionId);
    }

    public async Task<ValidationResult> ValidateUpdateMarkAsync(int submissionId, int mark)
    {
        var teacherIdClaim = _user.FindFirst(CustomClaimTypes.TeacherId)?.Value;
        if (teacherIdClaim is null)
        {
            return new ValidationResult(new ValidationError(nameof(_user),
                $"Current user does now have a claim of type {nameof(CustomClaimTypes.TeacherId)}"));
        }

        if (!int.TryParse(teacherIdClaim, out var teacherId))
        {
            return new ValidationResult(new ValidationError(nameof(teacherIdClaim),
                $"Current user`s claim of type {nameof(CustomClaimTypes.TeacherId)} must be number, not {teacherIdClaim}"));
        }

        return await ValidateUpdateMarkData(submissionId, mark, teacherId);
    }

    private async Task<ValidationResult> ValidateUpdateMarkData(int submissionId, int mark, int teacherId)
    {
        var submissionFromDb =
            await _parametersDataAccess.GetSingleAsync<TaskSubmission, TaskSubmissionValidationParameters>(
                new TaskSubmissionValidationParameters { TaskSubmissionId = submissionId, IncludeCourseTask = true });

        if (submissionFromDb.CourseTask!.MaxPoints < mark)
        {
            return new ValidationResult(new ValidationError(nameof(mark),
                $"Max mark for this task is {submissionFromDb.CourseTask.MaxPoints}, but you are passed {mark}"));
        }

        return await ValidateTeacher(
            _parametersDataAccess.GetByParameters<Teacher, TeacherByCourseParameters>(new TeacherByCourseParameters
                (submissionFromDb.CourseTask.CourseId)), teacherId);
    }

    private static async Task<ValidationResult> ValidateTeacher(IQueryable<Teacher> teachers, int teacherId)
    {
        if (await teachers.AllAsync(teacher => teacher.Id != teacherId))
        {
            return new ValidationResult(new ValidationError(nameof(teacherId),
                $"Current user is logged in as teacher with id = {teacherId}," +
                " but this teacher does not teaches submission author`s group"));
        }

        return new ValidationResult();
    }

    private async Task<ValidationResult> ValidateStudentId(int submissionId)
    {
        var studentIdClaimValidation = _user.ValidateParsableClaim<int>(CustomClaimTypes.StudentId);
        if (!studentIdClaimValidation.Result.IsValid)
        {
            return studentIdClaimValidation.Result;
        }

        var submissionFromDb =
            await _parametersDataAccess.GetSingleAsync<TaskSubmission, TaskSubmissionValidationParameters>(
                new TaskSubmissionValidationParameters { TaskSubmissionId = submissionId });

        var studentId = studentIdClaimValidation.ClaimValue;
        if (studentId != submissionFromDb.StudentId)
        {
            return new ValidationResult(new ValidationError(nameof(studentId),
                $"Current user is logged in as student with id = {studentId}, " +
                $"but to make changes to this submission user`s student id must be {submissionFromDb.StudentId}"));
        }

        return new ValidationResult();
    }
}