using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Messaging;
using ECampus.Shared.Messaging.TaskSubmissions;

namespace ECampus.Domain.Messaging;

public class CourseTaskMessagingService : IBaseService<CourseTaskDto>
{
    private readonly IBaseService<CourseTaskDto> _baseService;
    private readonly ICourseTaskMessageDataAccess _courseTaskMessageDataAccess;
    private readonly ISqsMessenger _sqsMessenger;

    public CourseTaskMessagingService(IBaseService<CourseTaskDto> baseService, ISqsMessenger sqsMessenger,
        ICourseTaskMessageDataAccess courseTaskMessageDataAccess)
    {
        _baseService = baseService;
        _sqsMessenger = sqsMessenger;
        _courseTaskMessageDataAccess = courseTaskMessageDataAccess;
    }

    public Task<CourseTaskDto> GetByIdAsync(int? id) => _baseService.GetByIdAsync(id);

    public async Task<CourseTaskDto> CreateAsync(CourseTaskDto entity)
    {
        var createdTask = await _baseService.CreateAsync(entity);
        var requiredData = await _courseTaskMessageDataAccess.LoadDataForSendMessage(createdTask.CourseId);
        if (!requiredData.studentEmails.Any())
        {
            return createdTask;
        }

        var message = new TaskCreated
        {
            StudentEmails = requiredData.studentEmails,
            MaxPoints = createdTask.MaxPoints,
            TaskName = createdTask.Name,
            Deadline = createdTask.Deadline,
            TaskType = createdTask.Type.ToString(),
            CourseName = requiredData.courseName
        };
        await _sqsMessenger.SendMessageAsync(message);
        return createdTask;
    }

    public Task<CourseTaskDto> UpdateAsync(CourseTaskDto entity) => _baseService.UpdateAsync(entity);

    public Task<CourseTaskDto> DeleteAsync(int? id) => _baseService.DeleteAsync(id);
}