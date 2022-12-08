using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Services
{
    public class ClassService : IClassService
    {
        private readonly ILogger<ClassService> _logger;
        private readonly IClassRepository _repository;
        private readonly IMapper _mapper;

        public ClassService(ILogger<ClassService> logger, IClassRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ClassDTO> CreateAsync(ClassDTO entity)
        {
            var @class = _mapper.Map<Class>(entity);
            return _mapper.Map<ClassDTO>(await _repository.CreateAsync(@class));
        }

        public Task DeleteAsync(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return _repository.DeleteAsync((int)id);
        }

        public async Task<ClassDTO> GetByIdAsync(int? id)
        {
            if(id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return _mapper.Map<ClassDTO>(await _repository.GetByIdAsync((int)id));
        }

        public async Task<Timetable> GetTimetableForAuditoryAsync(int auditoryId)
        {
            _logger.LogInformation("Getting auditory for group with id={id}", auditoryId);
            var timetable = await _repository.GetTimetableForAuditoryAsync(auditoryId);
            return _mapper.Map<Timetable>(timetable);
        }

        public async Task<Timetable> GetTimetableForGroupAsync(int groupId)
        {
            _logger.LogInformation("Getting timetable for group with id={id}", groupId);
            var timetable = await _repository.GetTimetableForGroupAsync(groupId);
            return _mapper.Map<Timetable>(timetable);
        }

        public async Task<Timetable> GetTimetableForTeacherAsync(int teacherId)
        {
            _logger.LogInformation("Getting teacher for group with id={id}", teacherId);
            var timetable = await _repository.GetTimetableForTeacherAsync(teacherId);
            return _mapper.Map<Timetable>(timetable);
        }

        public async Task<ClassDTO> UpdateAsync(ClassDTO entity)
        {
            var @class = _mapper.Map<Class>(entity);
            return _mapper.Map<ClassDTO>(await _repository.UpdateAsync(@class));
        }
    }
}
