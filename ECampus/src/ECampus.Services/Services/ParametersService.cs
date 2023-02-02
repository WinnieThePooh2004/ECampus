using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Shared.Data;
using ECampus.Shared.DataContainers;
using ECampus.Shared.QueryParameters;

namespace ECampus.Services.Services;

public class ParametersService<TDto, TParameters, TRepositoryModel> : IParametersService<TDto, TParameters>
    where TDto : class, IDataTransferObject, new()
    where TRepositoryModel : class, IModel, new()
    where TParameters : class, IQueryParameters<TRepositoryModel>, IQueryParameters
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
        var result = await _parametersDataAccessFacade.GetByParameters(parameters);
        return _mapper.Map<ListWithPaginationData<TDto>>(result);
    }
}