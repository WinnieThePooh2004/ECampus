using System.Security.Claims;
using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services;

public class TaskSubmissionService : ITaskSubmissionService
{
    private readonly ClaimsPrincipal _user;
    private readonly IMapper _mapper;
    private readonly IParametersDataAccessManager _parametersDataAccessManager;
    private readonly IDataAccessManager _dataAccessManager;

    public TaskSubmissionService(IHttpContextAccessor httpContextAccessor, IMapper mapper,
        IDataAccessManager dataAccessManager, IParametersDataAccessManager parametersDataAccessManager)
    {
        _mapper = mapper;
        _dataAccessManager = dataAccessManager;
        _parametersDataAccessManager = parametersDataAccessManager;
        _user = httpContextAccessor.HttpContext!.User;
    }

    public async Task<TaskSubmissionDto> UpdateContentAsync(int submissionId, string content)
    {
        var submission = await _dataAccessManager.GetByIdAsync<TaskSubmission>(submissionId)
                         ?? throw new ObjectNotFoundByIdException(typeof(TaskSubmission), submissionId);
        submission.SubmissionContent = content;
        await _dataAccessManager.SaveChangesAsync();
        return _mapper.Map<TaskSubmissionDto>(submission);
    }

    public async Task<TaskSubmissionDto> UpdateMarkAsync(int submissionId, int mark)
    {
        var submission = await _dataAccessManager.GetByIdAsync<TaskSubmission>(submissionId);
        submission.IsMarked = true;
        submission.TotalPoints = mark;
        await _dataAccessManager.SaveChangesAsync();
        return _mapper.Map<TaskSubmissionDto>(submission);
    }

    public async Task<TaskSubmissionDto> GetByIdAsync(int id)
    {
        return _mapper.Map<TaskSubmissionDto>(await _dataAccessManager.GetByIdAsync<TaskSubmission>(id));
    }

    public async Task<TaskSubmissionDto> GetByCourseAsync(int courseTaskId)
    {
        var currentStudentId = int.Parse(_user.FindFirst(CustomClaimTypes.StudentId)!.Value);
        
        return _mapper.Map<TaskSubmissionDto>(
            await _parametersDataAccessManager
                .GetByParameters<TaskSubmission, TaskSubmissionByStudentAndCourseParameters>(
                    new TaskSubmissionByStudentAndCourseParameters
                        { StudentId = currentStudentId, CourseTaskId = courseTaskId }).SingleOrDefaultAsync());
    }
}