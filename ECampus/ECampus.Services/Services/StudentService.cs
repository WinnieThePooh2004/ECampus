using AutoMapper;
using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;
using ECampus.Services.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services;

public class StudentService : IBaseService<StudentDto>
{
    private readonly IBaseService<StudentDto> _baseService;
    private readonly IDataAccessFacade _dataAccess;
    private readonly IMapper _mapper;

    public StudentService(IBaseService<StudentDto> baseService, IDataAccessFacade dataAccess, IMapper mapper)
    {
        _baseService = baseService;
        _dataAccess = dataAccess;
        _mapper = mapper;
    }

    public Task<StudentDto> GetByIdAsync(int id, CancellationToken token = default)
    {
        return _baseService.GetByIdAsync(id, token);
    }

    public async Task<StudentDto> CreateAsync(StudentDto entity, CancellationToken token = default)
    {
        var student = _mapper.Map<Student>(entity);
        student.Submissions = await _dataAccess
            .GetByParameters<CourseTask, TasksByGroupParameters>(new TasksByGroupParameters(entity.GroupId))
            .Select(task => new TaskSubmission { CourseTaskId = task.Id })
            .ToListAsync(token);

        var createdStudent = _dataAccess.Create(student);
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<StudentDto>(createdStudent);
    }

    public Task<StudentDto> UpdateAsync(StudentDto entity, CancellationToken token = default)
    {
        return _baseService.UpdateAsync(entity, token);
    }

    public Task<StudentDto> DeleteAsync(int id, CancellationToken token = default)
    {
        return _baseService.DeleteAsync(id, token);
    }
}