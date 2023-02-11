using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Contracts.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services;

public class StudentService : IBaseService<StudentDto>
{
    private readonly IBaseService<StudentDto> _baseService;
    private readonly IDataAccessManager _dataAccess;
    private readonly IMapper _mapper;

    public StudentService(IBaseService<StudentDto> baseService, IDataAccessManager dataAccess, IMapper mapper)
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

        var createdStudent = await _dataAccess.CreateAsync(student, token);
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