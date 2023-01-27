using System.Net;
using System.Security.Claims;
using AutoMapper;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Metadata;
using Microsoft.AspNetCore.Http;

namespace ECampus.Domain.Services;

[Inject(typeof(ITaskSubmissionService))]
public class TaskSubmissionService : ITaskSubmissionService
{
    private readonly ITaskSubmissionRepository _taskSubmissionRepository;
    private readonly ITaskSubmissionValidator _taskSubmissionValidator;
    private readonly ClaimsPrincipal _user;
    private readonly IMapper _mapper;

    public TaskSubmissionService(ITaskSubmissionRepository taskSubmissionRepository,
        ITaskSubmissionValidator taskSubmissionValidator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _taskSubmissionRepository = taskSubmissionRepository;
        _taskSubmissionValidator = taskSubmissionValidator;
        _mapper = mapper;
        _user = httpContextAccessor.HttpContext?.User ?? throw new HttpContextNotFoundExceptions();
    }

    public async Task UpdateContent(int submissionId, string content)
    {
        var validationResult = await _taskSubmissionValidator.ValidateUpdateContent(submissionId, content);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(typeof(TaskSubmissionDto), validationResult);
        }

        await _taskSubmissionRepository.UpdateContent(submissionId, content);
    }

    public async Task UpdateMark(int submissionId, int mark)
    {
        var validationResult = await _taskSubmissionValidator.ValidateUpdateMark(submissionId, mark);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(typeof(TaskSubmissionDto), validationResult);
        }

        await _taskSubmissionRepository.UpdateMark(submissionId, mark);
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
            await _taskSubmissionRepository.GetByStudentAndCourse(currentStudentId, courseTaskId));
    }
}