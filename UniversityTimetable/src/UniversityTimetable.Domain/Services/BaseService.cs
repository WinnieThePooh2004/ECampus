using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;

namespace UniversityTimetable.Domain.Services
{
    public class BaseService<TDTO, TRepositoryModel> : IBaseService<TDTO>
        where TRepositoryModel : class, IModel
        where TDTO : class, IDataTransferObject
    {
        private readonly IBaseRepository<TRepositoryModel> _repository;
        private readonly ILogger<BaseService<TDTO, TRepositoryModel>> _logger;
        private readonly IMapper _mapper;

        public BaseService(IBaseRepository<TRepositoryModel> repository, ILogger<BaseService<TDTO, TRepositoryModel>> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<TDTO> CreateAsync(TDTO entity)
        {
            var auditory = _mapper.Map<TRepositoryModel>(entity);
            return _mapper.Map<TDTO>(await _repository.CreateAsync(auditory));
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null)
            {
                _logger.LogAndThrowException(new NullIdException());
                return;
            }
            await _repository.DeleteAsync((int)id);
        }

        public async Task<TDTO> GetByIdAsync(int? id)
        {
            if (id is null)
            {
                _logger.LogAndThrowException(new NullIdException());
                return default!;
            }
            return _mapper.Map<TDTO>(await _repository.GetByIdAsync((int)id));
        }

        public async Task<TDTO> UpdateAsync(TDTO entity)
        {
            var auditory = _mapper.Map<TRepositoryModel>(entity);
            return _mapper.Map<TDTO>(await _repository.UpdateAsync(auditory));
        }
    }
}
