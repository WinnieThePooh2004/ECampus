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
    public class FacultacyService : IService<FacultacyDTO, FacultacyParameters>
    {
        private readonly ILogger<FacultacyService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Facultacy, FacultacyParameters> _repository;

        public FacultacyService(IRepository<Facultacy, FacultacyParameters> repository, IMapper mapper, ILogger<FacultacyService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FacultacyDTO> CreateAsync(FacultacyDTO entity)
        {
            var facultacy = _mapper.Map<Facultacy>(entity);
            return _mapper.Map<FacultacyDTO>(await _repository.CreateAsync(facultacy));
        }

        public async Task DeleteAsync(int? id)
        {
            if(id is null)
            {
                throw new Exception();
            }
            await _repository.DeleteAsync((int)id);
        }

        public async Task<FacultacyDTO> GetByIdAsync(int? id)
        {
            if (id is null)
            {
                throw new Exception();
            }
            return _mapper.Map<FacultacyDTO>(await _repository.GetByIdAsync((int)id));
        }

        public async Task<ListWithPaginationData<FacultacyDTO>> GetByParametersAsync(FacultacyParameters parameters)
        {
            _logger.LogInformation("Getting facultacies by parameters:{params}", parameters);
            var facultacies = await _repository.GetByParameters(parameters);
            _logger.LogInformation("By provided parameters found {count} facultacies", facultacies.Metadata.TotalCount);
            return _mapper.Map<ListWithPaginationData<FacultacyDTO>>(facultacies);
        }

        public async Task<FacultacyDTO> UpdateAsync(FacultacyDTO entity)
        {
            var facultacy = _mapper.Map<Facultacy>(entity);
            return _mapper.Map<FacultacyDTO>(await _repository.UpdateAsync(facultacy));
        }
    }
}
