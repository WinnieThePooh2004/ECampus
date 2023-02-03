using System.Net;
using System.Security.Claims;
using ECampus.Contracts.Services;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Validation;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Models;
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

    public Task<TaskSubmissionDto> GetByIdAsync(int id)
    {
        var role = _user.Claims;
        return _baseService.GetByIdAsync(id);
    }

    public async Task<TaskSubmissionDto> GetByCourseAsync(int courseTaskId)
    {
        var studentClaimIdValidation = _user.ValidateParsableClaim<int>(CustomClaimTypes.StudentId);
        if (!studentClaimIdValidation.Result.IsValid)
        {
            throw new DomainException(HttpStatusCode.Forbidden,
                "To view this submission you must be registered as student",
                studentClaimIdValidation.Result);
        }

        var submission = await _baseService.GetByCourseAsync(courseTaskId);
        if (submission.StudentId != studentClaimIdValidation.ClaimValue)
        {
            throw new DomainException(HttpStatusCode.Forbidden, "You can view only your submissions");
        }

        return submission;
    }
}