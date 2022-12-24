using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;

namespace UniversityTimetable.Domain.Services
{
    public class BaseService<TDto, TRepositoryModel> : IBaseService<TDto>
        where TRepositoryModel : class, IModel
        where TDto : class, IDataTransferObject
    {
        private readonly IBaseRepository<TRepositoryModel> _repository;
        private readonly ILogger<BaseService<TDto, TRepositoryModel>> _logger;
        private readonly IMapper _mapper;
        private readonly ICreateValidator<TDto> _createValidator;
        private readonly IUpdateValidator<TDto> _updateValidator;
        
        public BaseService(IBaseRepository<TRepositoryModel> repository, ILogger<BaseService<TDto, TRepositoryModel>> logger,
            IMapper mapper, ICreateValidator<TDto> createValidator, IUpdateValidator<TDto> updateValidator)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<TDto> CreateAsync(TDto entity)
        {
            var errors = await _createValidator.ValidateAsync(entity);
            if (errors.Any())
            {
                _logger.LogAndThrowException(new ValidationException(typeof(TDto), errors));
            }
            var auditory = _mapper.Map<TRepositoryModel>(entity);
            return _mapper.Map<TDto>(await _repository.CreateAsync(auditory));
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null)
            {
                throw new NullIdException();
            }
            await _repository.DeleteAsync((int)id);
        }

        public async Task<TDto> GetByIdAsync(int? id)
        {
            if (id is null)
            {
                throw new NullIdException();
            }
            return _mapper.Map<TDto>(await _repository.GetByIdAsync((int)id));
        }

        public async Task<TDto> UpdateAsync(TDto entity)
        {
            var errors = await _updateValidator.ValidateAsync(entity);
            if (errors.Any())
            {
                _logger.LogAndThrowException(new ValidationException(typeof(TDto), errors));
            }
            var auditory = _mapper.Map<TRepositoryModel>(entity);
            return _mapper.Map<TDto>(await _repository.UpdateAsync(auditory));
        }
    }
}
