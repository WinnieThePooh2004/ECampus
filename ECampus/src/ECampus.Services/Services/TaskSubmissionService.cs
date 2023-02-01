using System.Net;
using System.Security.Claims;
using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Core.Messages;
using ECampus.Core.Metadata;
using ECampus.Domain.Interfaces;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using Microsoft.AspNetCore.Http;

namespace ECampus.Services.Services;

[Inject(typeof(ITaskSubmissionService))]
public class TaskSubmissionService : ITaskSubmissionService
{
    private readonly ITaskSubmissionRepository _taskSubmissionRepository;
    private readonly ITaskSubmissionValidator _taskSubmissionValidator;
    private readonly ISnsMessenger _snsMessenger;
    private readonly ClaimsPrincipal _user;
    private readonly IMapper _mapper;

    public TaskSubmissionService(ITaskSubmissionRepository taskSubmissionRepository,
        ITaskSubmissionValidator taskSubmissionValidator,
        IHttpContextAccessor httpContextAccessor,
        ISnsMessenger snsMessenger, IMapper mapper)
    {
        _taskSubmissionRepository = taskSubmissionRepository;
        _taskSubmissionValidator = taskSubmissionValidator;
        _mapper = mapper;
        _snsMessenger = snsMessenger;
        _user = httpContextAccessor.HttpContext!.User;
    }

    public async Task UpdateContentAsync(int submissionId, string content)
    {
        var validationResult = await _taskSubmissionValidator.ValidateUpdateContentAsync(submissionId, content);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(typeof(TaskSubmissionDto), validationResult);
        }

        var submission = await _taskSubmissionRepository.UpdateContentAsync(submissionId, content);
        await _snsMessenger.PublishMessageAsync(new SubmissionEdited
        {
            UserEmail = submission.Student?.UserEmail,
            TaskName = submission.CourseTask!.Name
        });
    }

    public async Task UpdateMarkAsync(int submissionId, int mark)
    {
        var validationResult = await _taskSubmissionValidator.ValidateUpdateMarkAsync(submissionId, mark);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(typeof(TaskSubmissionDto), validationResult);
        }

        var submission = await _taskSubmissionRepository.UpdateMarkAsync(submissionId, mark);
        await _snsMessenger.PublishMessageAsync(new SubmissionMarked
        {
            TaskName = submission.CourseTask!.Name,
            UserEmail = submission.Student?.UserEmail,
            MaxPoints = submission.CourseTask.MaxPoints,
            ScoredPoints = submission.TotalPoints
        });
    }

    public async Task<TaskSubmissionDto> GetByIdAsync(int id)
    {
        return _mapper.Map<TaskSubmissionDto>(await _taskSubmissionRepository.GetByIdAsync(id));
    }

    public async Task<TaskSubmissionDto> GetByCourse(int courseTaskId)
    {
        var currentStudentIdClaim = _user.FindFirst(CustomClaimTypes.StudentId);
        if (currentStudentIdClaim is null || !int.TryParse(currentStudentIdClaim.Value, out var currentStudentId))
        {
            throw new DomainException(HttpStatusCode.Forbidden,
                $"Current user is not logged in as student or claim '{CustomClaimTypes.StudentId}' is not a number");
        }

        return _mapper.Map<TaskSubmissionDto>(
            await _taskSubmissionRepository.GetByStudentAndCourseAsync(currentStudentId, courseTaskId));
    }
}