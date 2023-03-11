using System.Security.Claims;
using ECampus.Core.Installers;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.Auth;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Validation.UniversalValidators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Validation.UpdateValidators;

[Inject(typeof(ITaskSubmissionValidator))]
public class TaskSubmissionValidator : FluentValidatorWrapper<TaskSubmissionDto>, ITaskSubmissionValidator
{
    private readonly ClaimsPrincipal _user;
    private readonly IDataAccessFacade _parametersDataAccess;

    public TaskSubmissionValidator(IHttpContextAccessor httpContextAccessor, IValidator<TaskSubmissionDto> validator,
        IDataAccessFacade parametersDataAccess) : base(validator)
    {
        _user = httpContextAccessor.HttpContext?.User ?? throw new HttpContextNotFoundExceptions();
        _parametersDataAccess = parametersDataAccess;
    }

    public async Task<ValidationResult> ValidateUpdateContentAsync(int submissionId, string content)
    {
        var errorsFromFluentValidator =
            await ValidateAsync(new TaskSubmissionDto { SubmissionContent = content });
        if (!errorsFromFluentValidator.IsValid)
        {
            return errorsFromFluentValidator;
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