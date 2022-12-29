using AutoMapper;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Domain.Services
{
    public class ParametersService<TDto, TParameters, TRepositoryModel> : IParametersService<TDto, TParameters>
        where TDto : class, IDataTransferObject, new()
        where TRepositoryModel : class, IModel, new()
        where TParameters : class, IQueryParameters<TRepositoryModel>
    {
        private readonly IParametersDataAccessFacade<TRepositoryModel, TParameters> _parametersDataAccessFacade;
        private readonly IMapper _mapper;
        private readonly IBaseService<TDto> _baseService;

        public ParametersService(IParametersDataAccessFacade<TRepositoryModel, TParameters> parametersDataAccessFacade,
            IMapper mapper, IBaseService<TDto> baseService)
        {
            _parametersDataAccessFacade = parametersDataAccessFacade;
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
            return _mapper.Map<ListWithPaginationData<TDto>>(await _parametersDataAccessFacade.GetByParameters(parameters));
        }

        public async Task<TDto> UpdateAsync(TDto entity)
            => await _baseService.UpdateAsync(entity);
    }
}
