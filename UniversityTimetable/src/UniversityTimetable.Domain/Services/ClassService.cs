using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Pagination;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Domain.Services
{
    public class ClassService : IService<ClassDTO, ClassParameters>
    {
        private readonly ILogger<ClassService> _logger;
        private readonly IRepository<Class, ClassParameters> _repository;
        private readonly IMapper _mapper;

        public ClassService(ILogger<ClassService> logger, IRepository<Class, ClassParameters> repository, IMapper mapper)
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

        public async Task<ListWithPaginationData<ClassDTO>> GetByParametersAsync(ClassParameters parameters)
        {
            return _mapper.Map<ListWithPaginationData<ClassDTO>>(await _repository.GetByParameters(parameters));
        }

        public async Task<ClassDTO> UpdateAsync(ClassDTO entity)
        {
            var @class = _mapper.Map<Class>(entity);
            return _mapper.Map<ClassDTO>(await _repository.UpdateAsync(@class));
        }
    }
}
