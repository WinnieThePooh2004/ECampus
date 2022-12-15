using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Domain.Services
{
    public class Service<TDTO, TParametrs, TRepositoryModel> : IService<TDTO, TParametrs>
        where TDTO : class, IDataTransferObject, new()
        where TRepositoryModel : class, IModel, new()
        where TParametrs : class, IQueryParameters<TRepositoryModel>
    {
        private readonly ILogger<Service<TDTO, TParametrs, TRepositoryModel>> _logger;
        private readonly IRepository<TRepositoryModel, TParametrs> _repository;
        private readonly IMapper _mapper;
        private readonly IBaseService<TDTO> _baseService;

        public Service(ILogger<Service<TDTO, TParametrs, TRepositoryModel>> logger, IRepository<TRepositoryModel, TParametrs> repository,
            IMapper mapper, IBaseService<TDTO> baseService)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _baseService = baseService;
        }

        public Task<TDTO> CreateAsync(TDTO entity)
            => _baseService.CreateAsync(entity);

        public Task DeleteAsync(int? id)
            => _baseService.DeleteAsync(id);

        public Task<TDTO> GetByIdAsync(int? id)
            => _baseService.GetByIdAsync(id);

        public async Task<ListWithPaginationData<TDTO>> GetByParametersAsync(TParametrs parameters)
        {
            return _mapper.Map<ListWithPaginationData<TDTO>>(await _repository.GetByParameters(parameters));
        }

        public async Task<TDTO> UpdateAsync(TDTO entity)
            => await _baseService.UpdateAsync(entity);
    }
}
