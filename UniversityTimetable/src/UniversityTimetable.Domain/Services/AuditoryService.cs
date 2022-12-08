using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Domain.Services
{
    public class AuditoryService : IService<AuditoryDTO, AuditoryParameters>
    {
        private readonly ILogger<AuditoryService> _logger;
        private readonly IRepository<Auditory, AuditoryParameters> _repository;
        private readonly IMapper _mapper;

        public AuditoryService(ILogger<AuditoryService> logger, IRepository<Auditory, AuditoryParameters> repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AuditoryDTO> CreateAsync(AuditoryDTO entity)
        {
            var auditory = _mapper.Map<Auditory>(entity);
            return _mapper.Map<AuditoryDTO>(await _repository.CreateAsync(auditory));
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            await _repository.DeleteAsync((int)id);
        }

        public async Task<AuditoryDTO> GetByIdAsync(int? id)
        {
            if(id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return _mapper.Map<AuditoryDTO>(await _repository.GetByIdAsync((int)id));
        }

        public async Task<ListWithPaginationData<AuditoryDTO>> GetByParametersAsync(AuditoryParameters parameters)
        {
            return _mapper.Map<ListWithPaginationData<AuditoryDTO>>(await _repository.GetByParameters(parameters));
        }

        public async Task<AuditoryDTO> UpdateAsync(AuditoryDTO entity)
        {
            var auditory = _mapper.Map<Auditory>(entity);
            return _mapper.Map<AuditoryDTO>(await _repository.UpdateAsync(auditory));
        }
    }
}
