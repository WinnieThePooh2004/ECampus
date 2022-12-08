using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Pagination;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Domain.Services
{
    public class TeacherService : IService<TeacherDTO, TeacherParameters>
    {
        private readonly IRepository<Teacher, TeacherParameters> _repository;
        private readonly ILogger<TeacherService> _logger;
        private readonly IMapper _mapper;

        public TeacherService(IRepository<Teacher, TeacherParameters> repository, ILogger<TeacherService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<TeacherDTO> CreateAsync(TeacherDTO entity)
        {
            _logger.LogInformation("Creating new teacher with departmentId={id}", entity.DepartmentId);
            var teacher = _mapper.Map<Teacher>(entity);
            await _repository.CreateAsync(teacher);
            return entity;
        }

        public Task DeleteAsync(int? id)
        {
            if(id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return _repository.DeleteAsync((int)id);
        }

        public async Task<TeacherDTO> GetByIdAsync(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return _mapper.Map<TeacherDTO>(await _repository.GetByIdAsync((int)id));
        }

        public async Task<ListWithPaginationData<TeacherDTO>> GetByParametersAsync(TeacherParameters parameters)
        {
            return _mapper.Map<ListWithPaginationData<TeacherDTO>>(await _repository.GetByParameters(parameters));
        }

        public async Task<TeacherDTO> UpdateAsync(TeacherDTO entity)
        {
            var teacher = _mapper.Map<Teacher>(entity);
            await _repository.UpdateAsync(teacher);
            return entity;
        }
    }
}
