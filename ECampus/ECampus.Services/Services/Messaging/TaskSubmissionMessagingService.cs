using ECampus.Core.Messages;
using ECampus.Domain.DataTransferObjects;
using ECampus.Services.Contracts.Messaging;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Messaging;

namespace ECampus.Services.Services.Messaging;

public class TaskSubmissionMessagingService : ITaskSubmissionService
{
    private readonly ITaskSubmissionService _baseService;
    private readonly ISnsMessenger _snsMessenger;

    public TaskSubmissionMessagingService(ITaskSubmissionService baseService, ISnsMessenger snsMessenger)
    {
        _baseService = baseService;
        _snsMessenger = snsMessenger;
    }

    public async Task<TaskSubmissionDto> UpdateContentAsync(int submissionId, string content,
        CancellationToken token = default)
    {
        var submission = await _baseService.UpdateContentAsync(submissionId, content, token);
        await _snsMessenger.PublishMessageAsync(new SubmissionEdited
        {
            UserEmail = submission.Student?.UserEmail,
            TaskName = submission.CourseTask!.Name
        });
        return submission;
    }

    public async Task<TaskSubmissionDto> UpdateMarkAsync(int submissionId, int mark, CancellationToken token = default)
    {
        var submission = await _baseService.UpdateMarkAsync(submissionId, mark, token);
        await _snsMessenger.PublishMessageAsync(new SubmissionMarked
        {
            TaskName = submission.CourseTask!.Name,
            UserEmail = submission.Student?.UserEmail,
            MaxPoints = submission.CourseTask.MaxPoints,
            ScoredPoints = submission.TotalPoints
        });
        return submission;
    }

    public Task<TaskSubmissionDto> GetByIdAsync(int id, CancellationToken token = default) =>
        _baseService.GetByIdAsync(id, token);

    public Task<TaskSubmissionDto> GetByCourseAsync(int courseTaskId, CancellationToken token = default) =>
        _baseService.GetByCourseAsync(courseTaskId, token);
}