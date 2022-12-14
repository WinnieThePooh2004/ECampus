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
    public class SubjectService : IService<SubjectDTO, SubjectParameters>

    {
        private readonly ILogger<SubjectService> _logger;
        private readonly IRepository<Subject, SubjectParameters> _repository;
        private readonly IMapper _mapper;

        public SubjectService(ILogger<SubjectService> logger, IRepository<Subject, SubjectParameters> repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SubjectDTO> CreateAsync(SubjectDTO entity)
        {
            var Subject = _mapper.Map<Subject>(entity);
            return _mapper.Map<SubjectDTO>(await _repository.CreateAsync(Subject));
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            await _repository.DeleteAsync((int)id);
        }

        public async Task<SubjectDTO> GetByIdAsync(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return _mapper.Map<SubjectDTO>(await _repository.GetByIdAsync((int)id));
        }

        public async Task<ListWithPaginationData<SubjectDTO>> GetByParametersAsync(SubjectParameters parameters)
        {
            return _mapper.Map<ListWithPaginationData<SubjectDTO>>(await _repository.GetByParameters(parameters));
        }

        public async Task<SubjectDTO> UpdateAsync(SubjectDTO entity)
        {
            var Subject = _mapper.Map<Subject>(entity);
            return _mapper.Map<SubjectDTO>(await _repository.UpdateAsync(Subject));
        }
    }
}
