using System.Security.Claims;
using ECampus.Core.Metadata;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Metadata;
using ECampus.Shared.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Domain.Validation.UpdateValidators;

[Inject(typeof(ITaskSubmissionValidator))]
public class TaskSubmissionValidator : ITaskSubmissionValidator
{
    private readonly ClaimsPrincipal _user;
    private readonly ITaskSubmissionDataValidator _taskSubmissionDataValidator;
    private readonly IValidator<TaskSubmissionDto> _validator;

    public TaskSubmissionValidator(IHttpContextAccessor httpContextAccessor,
        ITaskSubmissionDataValidator taskSubmissionDataValidator, IValidator<TaskSubmissionDto> validator)
    {
        _user = httpContextAccessor.HttpContext?.User ?? throw new HttpContextNotFoundExceptions();
        _taskSubmissionDataValidator = taskSubmissionDataValidator;
        _validator = validator;
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

        var submissionFromDb = await _taskSubmissionDataValidator.LoadSubmissionData(submissionId);
        if (submissionFromDb.CourseTask!.MaxPoints < mark)
        {
            return new ValidationResult(new ValidationError(nameof(mark),
                $"Max mark for this task is {submissionFromDb.CourseTask.MaxPoints}, but you are passed {mark}"));
        }

        return await _taskSubmissionDataValidator.ValidateTeacherId(submissionId, teacherId);
    }

    private async Task<ValidationResult> ValidateStudentId(int submissionId)
    {
        var studentIdClaim = _user.FindFirst(CustomClaimTypes.StudentId)?.Value;
        if (studentIdClaim is null)
        {
            return new ValidationResult(new ValidationError(nameof(_user),
                $"Current user does now have a claim of type {nameof(CustomClaimTypes.StudentId)}"));
        }

        if (!int.TryParse(studentIdClaim, out var studentId))
        {
            return new ValidationResult(new ValidationError(nameof(studentIdClaim),
                $"Current user`s claim of type {nameof(CustomClaimTypes.StudentId)} must be number, not {studentIdClaim}"));
        }

        var submissionFromDb = await _taskSubmissionDataValidator.LoadSubmissionData(submissionId);
        if (studentId != submissionFromDb.StudentId)
        {
            return new ValidationResult(new ValidationError(nameof(studentId),
                $"Current user is logged in as student with id = {studentId}, " +
                $"but to make changes to this submission user`s student id must be {submissionFromDb.StudentId}"));
        }

        return new ValidationResult();
    }
}