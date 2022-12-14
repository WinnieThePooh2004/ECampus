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
    public class FacultyService : IService<FacultyDTO, FacultyParameters>
    {
        private readonly ILogger<FacultyService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Faculty, FacultyParameters> _repository;

        public FacultyService(IRepository<Faculty, FacultyParameters> repository, IMapper mapper, ILogger<FacultyService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FacultyDTO> CreateAsync(FacultyDTO entity)
        {
            var facultacy = _mapper.Map<Faculty>(entity);
            return _mapper.Map<FacultyDTO>(await _repository.CreateAsync(facultacy));
        }

        public async Task DeleteAsync(int? id)
        {
            if(id is null)
            {
                throw new Exception();
            }
            await _repository.DeleteAsync((int)id);
        }

        public async Task<FacultyDTO> GetByIdAsync(int? id)
        {
            if (id is null)
            {
                throw new Exception();
            }
            return _mapper.Map<FacultyDTO>(await _repository.GetByIdAsync((int)id));
        }

        public async Task<ListWithPaginationData<FacultyDTO>> GetByParametersAsync(FacultyParameters parameters)
        {
            _logger.LogInformation("Getting facultacies by parameters:{params}", parameters);
            var facultacies = await _repository.GetByParameters(parameters);
            _logger.LogInformation("By provided parameters found {count} facultacies", facultacies.Metadata.TotalCount);
            return _mapper.Map<ListWithPaginationData<FacultyDTO>>(facultacies);
        }

        public async Task<FacultyDTO> UpdateAsync(FacultyDTO entity)
        {
            var facultacy = _mapper.Map<Faculty>(entity);
            return _mapper.Map<FacultyDTO>(await _repository.UpdateAsync(facultacy));
        }
    }
}
