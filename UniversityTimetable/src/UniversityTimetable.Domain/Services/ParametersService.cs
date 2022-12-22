using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Domain.Services
{
    public class ParametersService<TDto, TParameters, TRepositoryModel> : IParametersService<TDto, TParameters>
        where TDto : class, IDataTransferObject, new()
        where TRepositoryModel : class, IModel, new()
        where TParameters : class, IQueryParameters<TRepositoryModel>
    {
        private readonly ILogger<ParametersService<TDto, TParameters, TRepositoryModel>> _logger;
        private readonly IParametersRepository<TRepositoryModel, TParameters> _parametersRepository;
        private readonly IMapper _mapper;
        private readonly IBaseService<TDto> _baseService;

        public ParametersService(ILogger<ParametersService<TDto, TParameters, TRepositoryModel>> logger, IParametersRepository<TRepositoryModel, TParameters> parametersRepository,
            IMapper mapper, IBaseService<TDto> baseService)
        {
            _logger = logger;
            _parametersRepository = parametersRepository;
            _mapper = mapper;
            _baseService = baseService;
        }

        public Task<TDto> CreateAsync(TDto entity)
            => _baseService.CreateAsync(entity);

        public Task DeleteAsync(int? id)
            => _baseService.DeleteAsync(id);

        public Task<TDto> GetByIdAsync(int? id)
            => _baseService.GetByIdAsync(id);

        public async Task<ListWithPaginationData<TDto>> GetByParametersAsync(TParameters parameters)
        {
            return _mapper.Map<ListWithPaginationData<TDto>>(await _parametersRepository.GetByParameters(parameters));
        }

        public async Task<TDto> UpdateAsync(TDto entity)
            => await _baseService.UpdateAsync(entity);
    }
}
