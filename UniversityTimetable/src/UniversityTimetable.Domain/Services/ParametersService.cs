using AutoMapper;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Domain.Services;

public class ParametersService<TDto, TParameters, TRepositoryModel> : IParametersService<TDto, TParameters>
    where TDto : class, IDataTransferObject, new()
    where TRepositoryModel : class, IModel, new()
    where TParameters : class, IQueryParameters<TRepositoryModel>
{
    private readonly IParametersDataAccessFacade<TRepositoryModel, TParameters> _parametersDataAccessFacade;
    private readonly IMapper _mapper;

    public ParametersService(IParametersDataAccessFacade<TRepositoryModel, TParameters> parametersDataAccessFacade,
        IMapper mapper)
    {
        _parametersDataAccessFacade = parametersDataAccessFacade;
        _mapper = mapper;
    }

    public async Task<ListWithPaginationData<TDto>> GetByParametersAsync(TParameters parameters)
    {
        return _mapper.Map<ListWithPaginationData<TDto>>(await _parametersDataAccessFacade.GetByParameters(parameters));
    }
}