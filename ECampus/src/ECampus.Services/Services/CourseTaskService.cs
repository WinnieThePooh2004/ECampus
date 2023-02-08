using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Contracts.Services;
using ECampus.Core.Messages;
using ECampus.Domain.Interfaces;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services;

public class CourseTaskService : IBaseService<CourseTaskDto>
{
    private readonly IBaseService<CourseTaskDto> _baseService;
    private readonly ISnsMessenger _snsMessenger;
    private readonly IDataAccessManagerFactory _dataAccessFactory;
    private readonly IMapper _mapper;

    public CourseTaskService(IBaseService<CourseTaskDto> baseService, ISnsMessenger snsMessenger,
        IDataAccessManagerFactory dataAccessFactory, IMapper mapper)
    {
        _baseService = baseService;
        _snsMessenger = snsMessenger;
        _dataAccessFactory = dataAccessFactory;
        _mapper = mapper;
    }

    public Task<CourseTaskDto> GetByIdAsync(int id) => _baseService.GetByIdAsync(id);

    public async Task<CourseTaskDto> CreateAsync(CourseTaskDto entity)
    {
        var task = _mapper.Map<CourseTask>(entity);
        var relatedStudents = await _dataAccessFactory.Complex
            .GetByParameters<Student, StudentsByCourseParameters>(new StudentsByCourseParameters
                { CourseId = entity.CourseId }).ToListAsync();
        
        task.Submissions = relatedStudents.Select(student => new TaskSubmission{StudentId = student.Id}).ToList();
        await _dataAccessFactory.Primitive.CreateAsync(task);
        await _dataAccessFactory.Primitive.SaveChangesAsync();
        
        await PublishMessage(entity, task, relatedStudents);
        return _mapper.Map<CourseTaskDto>(task);
    }

    private async Task PublishMessage(CourseTaskDto entity, CourseTask task, List<Student> relatedStudents)
    {
        var studentEmails = relatedStudents
            .Where(student => student.UserEmail is not null)
            .Select(student => student.UserEmail)
            .ToList();
        if (!studentEmails.Any())
        {
            return;
        }

        var course = await _dataAccessFactory.Primitive.GetByIdAsync<Course>(entity.CourseId);
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

    public Task<CourseTaskDto> UpdateAsync(CourseTaskDto entity) => _baseService.UpdateAsync(entity);

    public Task<CourseTaskDto> DeleteAsync(int id) => _baseService.DeleteAsync(id);
}