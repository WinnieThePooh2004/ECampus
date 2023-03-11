using AutoMapper;
using ECampus.Core.Messages;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Services.Contracts.Messaging;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Messaging;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services.Messaging;

public class CourseTaskService : IBaseService<CourseTaskDto>
{
    private readonly IBaseService<CourseTaskDto> _baseService;
    private readonly ISnsMessenger _snsMessenger;
    private readonly IDataAccessFacade _dataAccessFactory;
    private readonly IMapper _mapper;

    public CourseTaskService(IBaseService<CourseTaskDto> baseService, ISnsMessenger snsMessenger,
        IDataAccessFacade dataAccessFactory, IMapper mapper)
    {
        _baseService = baseService;
        _snsMessenger = snsMessenger;
        _dataAccessFactory = dataAccessFactory;
        _mapper = mapper;
    }

    public Task<CourseTaskDto> GetByIdAsync(int id, CancellationToken token = default) =>
        _baseService.GetByIdAsync(id, token);

    public async Task<CourseTaskDto> CreateAsync(CourseTaskDto dto, CancellationToken token = default)
    {
        var task = _mapper.Map<CourseTask>(dto);
        var relatedStudents = await _dataAccessFactory
            .GetByParameters<Student, StudentsByCourseParameters>(new StudentsByCourseParameters(dto.CourseId))
            .ToListAsync(token);

        task.Submissions = relatedStudents.Select(student => new TaskSubmission { StudentId = student.Id }).ToList();
        _dataAccessFactory.Create(task);
        await _dataAccessFactory.SaveChangesAsync(token);

        await PublishMessage(dto, task, relatedStudents, token);
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

        var course = await _dataAccessFactory.PureByIdAsync<Course>(entity.CourseId, token);
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

    public Task<CourseTaskDto> UpdateAsync(CourseTaskDto dto, CancellationToken token = default) =>
        _baseService.UpdateAsync(dto, token);

    public Task<CourseTaskDto> DeleteAsync(int id, CancellationToken token = default) =>
        _baseService.DeleteAsync(id, token);
}