using System.Net;
using System.Security.Claims;
using ECampus.Contracts.Services;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Validation;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Services.Services.ValidationServices;

public class TaskSubmissionValidationService : ITaskSubmissionService
{
    private readonly ITaskSubmissionService _baseService;
    private readonly ITaskSubmissionValidator _validator;
    private readonly ClaimsPrincipal _user;

    public TaskSubmissionValidationService(ITaskSubmissionService baseService, ITaskSubmissionValidator validator,
        IHttpContextAccessor httpContextAccessor)
    {
        _baseService = baseService;
        _validator = validator;
        _user = httpContextAccessor.HttpContext!.User;
    }

    public async Task<TaskSubmissionDto> UpdateContentAsync(int submissionId, string content)
    {
        var validationResult = await _validator.ValidateUpdateContentAsync(submissionId, content);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(typeof(TaskSubmissionDto), validationResult);
        }

        return await _baseService.UpdateContentAsync(submissionId, content);
    }

    public async Task<TaskSubmissionDto> UpdateMarkAsync(int submissionId, int mark)
    {
        var validationResult = await _validator.ValidateUpdateMarkAsync(submissionId, mark);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(typeof(TaskSubmission), validationResult);
        }

        return await _baseService.UpdateMarkAsync(submissionId, mark);
    }

    public async Task<TaskSubmissionDto> GetByIdAsync(int id)
    {
        var role = _user.ValidateEnumClaim<UserRole>(ClaimTypes.Role);
        if (!role.Result.IsValid)
        {
            throw new ValidationException(typeof(ClaimsPrincipal), role.Result);
        }
        var submission = await _baseService.GetByIdAsync(id);
        if (role.ClaimValue is UserRole.Admin or UserRole.Teacher)
        {
            return submission;
        }
        ValidateStudentId(submission);
        return submission;
    }

    public async Task<TaskSubmissionDto> GetByCourseAsync(int courseTaskId)
    {
        var submission = await _baseService.GetByCourseAsync(courseTaskId);
        ValidateStudentId(submission);
        return submission;
    }

    private void ValidateStudentId(TaskSubmissionDto submission)
    {
        if (submission.StudentId != ValidateStudentClaim().ClaimValue)
        {
            throw new DomainException(HttpStatusCode.Forbidden, "You can view only your submissions");
        }
    }

    private (ValidationResult Result, int? ClaimValue) ValidateStudentClaim()
    {
        var studentClaimIdValidation = _user.ValidateParsableClaim<int>(CustomClaimTypes.StudentId);
        if (!studentClaimIdValidation.Result.IsValid)
        {
            throw new DomainException(HttpStatusCode.Forbidden,
                $"You are registered as student, but your claim '{nameof(UserRole.Student)}' is not valid",
                studentClaimIdValidation.Result);
        }

        return studentClaimIdValidation;
    }
}