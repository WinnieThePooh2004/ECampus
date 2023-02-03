using ECampus.Contracts.Services;
using ECampus.Core.Messages;
using ECampus.Domain.Interfaces;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.Domain.Messaging;

public class TaskSubmissionMessagingService : ITaskSubmissionService
{
    private readonly ITaskSubmissionService _baseService;
    private readonly ISnsMessenger _snsMessenger;

    public TaskSubmissionMessagingService(ITaskSubmissionService baseService, ISnsMessenger snsMessenger)
    {
        _baseService = baseService;
        _snsMessenger = snsMessenger;
    }

    public async Task<TaskSubmissionDto> UpdateContentAsync(int submissionId, string content)
    {
        var submission = await _baseService.UpdateContentAsync(submissionId, content);
        await _snsMessenger.PublishMessageAsync(new SubmissionEdited
        {
            UserEmail = submission.Student?.UserEmail,
            TaskName = submission.CourseTask!.Name
        });
        return submission;
    }

    public async Task<TaskSubmissionDto> UpdateMarkAsync(int submissionId, int mark)
    {
        var submission = await _baseService.UpdateMarkAsync(submissionId, mark);
        await _snsMessenger.PublishMessageAsync(new SubmissionMarked
        {
            TaskName = submission.CourseTask!.Name,
            UserEmail = submission.Student?.UserEmail,
            MaxPoints = submission.CourseTask.MaxPoints,
            ScoredPoints = submission.TotalPoints
        });
        return submission;
    }

    public Task<TaskSubmissionDto> GetByIdAsync(int id) => _baseService.GetByIdAsync(id);

    public Task<TaskSubmissionDto> GetByCourseAsync(int courseTaskId) => _baseService.GetByCourseAsync(courseTaskId);
}