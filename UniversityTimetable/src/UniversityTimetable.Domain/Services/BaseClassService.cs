using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Services
{
    public class BaseClassService : IBaseService<ClassDTO>
    {
        private readonly IBaseService<ClassDTO> _baseService;
        private readonly ILogger<BaseClassService> _logger;
        private readonly IClassRepository _repository;
        private readonly IMapper _mapper;
        public BaseClassService(IBaseService<ClassDTO> baseService, ILogger<BaseClassService> logger, IClassRepository repository, IMapper mapper)
        {
            _baseService = baseService;
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ClassDTO> CreateAsync(ClassDTO entity)
        {
            var errors = await _repository.ValidateAsync(_mapper.Map<Class>(entity));
            if (errors.Any())
            {
                _logger.LogAndThrowException(new DomainException(HttpStatusCode.BadRequest, "One or move validation errors happened", errors));
            }
            return await _baseService.CreateAsync(entity);
        }

        public Task DeleteAsync(int? id)
            => _baseService.DeleteAsync(id);

        public Task<ClassDTO> GetByIdAsync(int? id)
            => _baseService.GetByIdAsync(id);

        public async Task<ClassDTO> UpdateAsync(ClassDTO entity)
        {
            var errors = await _repository.ValidateAsync(_mapper.Map<Class>(entity));
            if (errors.Any())
            {
                _logger.LogAndThrowException(new DomainException(HttpStatusCode.BadRequest, "One or move validation errors happened", errors));
            }
            return await _baseService.UpdateAsync(entity);
        }
    }
}
