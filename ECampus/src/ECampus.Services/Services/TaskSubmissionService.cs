using System.Net;
using System.Security.Claims;
using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Contracts.Services;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services;

public class TaskSubmissionService : ITaskSubmissionService
{
    private readonly ClaimsPrincipal _user;
    private readonly IMapper _mapper;
    private readonly IDataAccessManager _dataAccess;

    public TaskSubmissionService(IHttpContextAccessor httpContextAccessor, IMapper mapper,
        IDataAccessManager dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
        _user = httpContextAccessor.HttpContext!.User;
    }

    public async Task<TaskSubmissionDto> UpdateContentAsync(int submissionId, string content)
    {
        var submission = await _dataAccess.GetByIdAsync<TaskSubmission>(submissionId);
        submission.SubmissionContent = content;
        await _dataAccess.SaveChangesAsync();
        return _mapper.Map<TaskSubmissionDto>(submission);
    }

    public async Task<TaskSubmissionDto> UpdateMarkAsync(int submissionId, int mark)
    {
        var submission = await _dataAccess.GetByIdAsync<TaskSubmission>(submissionId);
        submission.IsMarked = true;
        submission.TotalPoints = mark;
        await _dataAccess.SaveChangesAsync();
        return _mapper.Map<TaskSubmissionDto>(submission);
    }

    public async Task<TaskSubmissionDto> GetByIdAsync(int id)
    {
        return _mapper.Map<TaskSubmissionDto>(await _dataAccess.GetByIdAsync<TaskSubmission>(id));
    }

    public async Task<TaskSubmissionDto> GetByCourseAsync(int courseTaskId)
    {
        var currentStudentId = int.Parse(_user.FindFirst(CustomClaimTypes.StudentId)!.Value);

        return _mapper.Map<TaskSubmissionDto>(
            await _dataAccess
                .GetByParameters<TaskSubmission, TaskSubmissionByStudentAndCourseParameters>(
                    new TaskSubmissionByStudentAndCourseParameters
                        { StudentId = currentStudentId, CourseTaskId = courseTaskId }).SingleOrDefaultAsync() ??
            throw new InfrastructureExceptions(HttpStatusCode.NotFound, 
                $"There is not any submissions with StudentId={currentStudentId} and TaskId={courseTaskId}"));
    }
}