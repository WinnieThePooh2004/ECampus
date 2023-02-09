using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Contracts.Services;
using ECampus.Core.Messages;
using ECampus.Domain.Interfaces;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services.Messaging;

public class CourseTaskService : IBaseService<CourseTaskDto>
{
    private readonly IBaseService<CourseTaskDto> _baseService;
    private readonly ISnsMessenger _snsMessenger;
    private readonly IDataAccessManager _dataAccessFactory;
    private readonly IMapper _mapper;

    public CourseTaskService(IBaseService<CourseTaskDto> baseService, ISnsMessenger snsMessenger,
        IDataAccessManager dataAccessFactory, IMapper mapper)
    {
        _baseService = baseService;
        _snsMessenger = snsMessenger;
        _dataAccessFactory = dataAccessFactory;
        _mapper = mapper;
    }

    public Task<CourseTaskDto> GetByIdAsync(int id, CancellationToken token = default) =>
        _baseService.GetByIdAsync(id, token);

    public async Task<CourseTaskDto> CreateAsync(CourseTaskDto entity, CancellationToken token = default)
    {
        var task = _mapper.Map<CourseTask>(entity);
        var relatedStudents = await _dataAccessFactory
            .GetByParameters<Student, StudentsByCourseParameters>(new StudentsByCourseParameters(entity.CourseId))
            .ToListAsync(token);

        task.Submissions = relatedStudents.Select(student => new TaskSubmission { StudentId = student.Id }).ToList();
        await _dataAccessFactory.CreateAsync(task, token);
        await _dataAccessFactory.SaveChangesAsync(token);

        await PublishMessage(entity, task, relatedStudents, token);
        return _mapper.Map<CourseTaskDto>(task);
    }

    private async Task PublishMessage(CourseTaskDto entity, CourseTask task, List<Student> relatedStudents,
        CancellationToken token = default)
    {
        var studentEmails = relatedStudents
            .Where(student => student.UserEmail is not null)
            .Select(student => student.UserEmail)
            .ToList();
        if (!studentEmails.Any())
        {
            return;
        }

        var course = await _dataAccessFactory.GetPureByIdAsync<Course>(entity.CourseId, token);
        var message = new TaskCreated
        {
            StudentEmails = studentEmails,
            MaxPoints = task.MaxPoints,
            TaskName = task.Name,
            Deadline = task.Deadline,
            TaskType = task.Type.ToString(),
            CourseName = course.Name
        };
        await _snsMessenger.PublishMessageAsync(message);
    }

    public Task<CourseTaskDto> UpdateAsync(CourseTaskDto entity, CancellationToken token = default) =>
        _baseService.UpdateAsync(entity, token);

    public Task<CourseTaskDto> DeleteAsync(int id, CancellationToken token = default) =>
        _baseService.DeleteAsync(id, token);
}