using System.Net;
using System.Security.Claims;
using AutoMapper;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.Auth;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Exceptions.InfrastructureExceptions;
using ECampus.Domain.Models;
using ECampus.Services.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services;

public class TaskSubmissionService : ITaskSubmissionService
{
    private readonly ClaimsPrincipal _user;
    private readonly IMapper _mapper;
    private readonly IDataAccessFacade _dataAccess;

    public TaskSubmissionService(IHttpContextAccessor httpContextAccessor, IMapper mapper,
        IDataAccessFacade dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
        _user = httpContextAccessor.HttpContext!.User;
    }

    public async Task<TaskSubmissionDto> UpdateContentAsync(int submissionId, string content, CancellationToken token = default)
    {
        var submission = await _dataAccess.GetByIdAsync<TaskSubmission>(submissionId, token);
        submission.SubmissionContent = content;
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<TaskSubmissionDto>(submission);
    }

    public async Task<TaskSubmissionDto> UpdateMarkAsync(int submissionId, int mark, CancellationToken token = default)
    {
        var submission = await _dataAccess.GetByIdAsync<TaskSubmission>(submissionId, token);
        submission.IsMarked = true;
        submission.TotalPoints = mark;
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<TaskSubmissionDto>(submission);
    }

    public async Task<TaskSubmissionDto> GetByIdAsync(int id, CancellationToken token = default)
    {
        return _mapper.Map<TaskSubmissionDto>(await _dataAccess.GetByIdAsync<TaskSubmission>(id, token));
    }

    public async Task<TaskSubmissionDto> GetByCourseAsync(int courseTaskId, CancellationToken token = default)
    {
        var currentStudentId = int.Parse(_user.FindFirst(CustomClaimTypes.StudentId)!.Value);

        return _mapper.Map<TaskSubmissionDto>(
            await _dataAccess
                .GetByParameters<TaskSubmission, TaskSubmissionByStudentAndCourseParameters>(
                    new TaskSubmissionByStudentAndCourseParameters
                        { StudentId = currentStudentId, CourseTaskId = courseTaskId }).SingleOrDefaultAsync(token) ??
            throw new InfrastructureExceptions(HttpStatusCode.NotFound, 
                $"There is not any submissions with StudentId={currentStudentId} and TaskId={courseTaskId}"));
    }
}