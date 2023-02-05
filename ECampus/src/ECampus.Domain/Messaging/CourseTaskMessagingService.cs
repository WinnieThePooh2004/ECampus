using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Core.Messages;
using ECampus.Domain.Interfaces;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;
using ECampus.Contracts.DataSelectParameters;

namespace ECampus.Domain.Messaging;

public class CourseTaskMessagingService : IBaseService<CourseTaskDto>
{
    private readonly IBaseService<CourseTaskDto> _baseService;
    private readonly ISnsMessenger _snsMessenger;
    private readonly IDataAccessManagerFactory _dataAccessFactory;

    public CourseTaskMessagingService(IBaseService<CourseTaskDto> baseService, ISnsMessenger snsMessenger,
        IDataAccessManagerFactory dataAccessFactory)
    {
        _baseService = baseService;
        _snsMessenger = snsMessenger;
        _dataAccessFactory = dataAccessFactory;
    }

    public Task<CourseTaskDto> GetByIdAsync(int id) => _baseService.GetByIdAsync(id);

    public async Task<CourseTaskDto> CreateAsync(CourseTaskDto entity)
    {
        var createdTask = await _baseService.CreateAsync(entity);
        var course = await _dataAccessFactory.Primitive.GetByIdAsync<Course>(entity.CourseId);
        var studentEmails = await _dataAccessFactory.Complex
            .GetByParameters<Student, StudentsByCourseParameters>(new StudentsByCourseParameters
                { CourseId = entity.CourseId })
            .Where(student => student.UserEmail != null)
            .Select(student => student.UserEmail)
            .ToListAsync();
        
        if (!studentEmails.Any())
        {
            return createdTask;
        }

        var message = new TaskCreated
        {
            StudentEmails = studentEmails,
            MaxPoints = createdTask.MaxPoints,
            TaskName = createdTask.Name,
            Deadline = createdTask.Deadline,
            TaskType = createdTask.Type.ToString(),
            CourseName = course.Name
        };
        await _snsMessenger.PublishMessageAsync(message);
        return createdTask;
    }

    public Task<CourseTaskDto> UpdateAsync(CourseTaskDto entity) => _baseService.UpdateAsync(entity);

    public Task<CourseTaskDto> DeleteAsync(int id) => _baseService.DeleteAsync(id);
}