using AutoMapper;
using FluentValidation.Results;
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
        private readonly IBaseService<ClassDTO> _baseService;

        public ClassService(ILogger<ClassService> logger, IClassRepository repository, IMapper mapper, IBaseService<ClassDTO> baseService)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _baseService = baseService;
        }

        public Task<ClassDTO> CreateAsync(ClassDTO entity)
            => _baseService.CreateAsync(entity);

        public Task DeleteAsync(int? id)
            => _baseService.DeleteAsync(id);

        public Task<ClassDTO> GetByIdAsync(int? id)
            => _baseService.GetByIdAsync(id);

        public async Task<Timetable> GetTimetableForAuditoryAsync(int auditoryId)
        {
            _logger.LogInformation("Getting auditory for group with id={id}", auditoryId);
            var timetable = await _repository.GetTimetableForAuditoryAsync(auditoryId);
            return new Timetable(_mapper.Map<IEnumerable<ClassDTO>>(timetable.Classes))
            {
                Auditory = _mapper.Map<AuditoryDTO>(timetable.Auditory),
            };
        }

        public async Task<Timetable> GetTimetableForGroupAsync(int groupId)
        {
            _logger.LogInformation("Getting timetable for group with id={id}", groupId);
            var timetable = await _repository.GetTimetableForGroupAsync(groupId);
            return new Timetable(_mapper.Map<IEnumerable<ClassDTO>>(timetable.Classes))
            {
                Group = _mapper.Map<GroupDTO>(timetable.Group),
            };
        }

        public async Task<Timetable> GetTimetableForTeacherAsync(int teacherId)
        {
            _logger.LogInformation("Getting teacher for group with id={id}", teacherId);
            var timetable = await _repository.GetTimetableForTeacherAsync(teacherId);
            return new Timetable(_mapper.Map<IEnumerable<ClassDTO>>(timetable.Classes))
            {
                Teacher = _mapper.Map<TeacherDTO>(timetable.Teacher),
            };
        }

        public Task<ClassDTO> UpdateAsync(ClassDTO entity) 
            => _baseService.UpdateAsync(entity);

        public Task<Dictionary<string, string>> ValidateAsync(ClassDTO @class)
        {
            return _repository.ValidateAsync(_mapper.Map<Class>(@class));
        }
    }
}
