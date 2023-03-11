using AutoMapper;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Services.Contracts.Services;
using ECampus.Shared.Data;
using ECampus.Shared.DataContainers;
using ECampus.Shared.Extensions;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services;

public class ParametersService<TDto, TParameters, TRepositoryModel> : IParametersService<TDto, TParameters>
    where TDto : class, IDataTransferObject, new()
    where TRepositoryModel : class, IModel, new()
    where TParameters : class, IDataSelectParameters<TRepositoryModel>, IQueryParameters<TDto>
{
    private readonly IDataAccessFacade _dataAccess;
    private readonly IMapper _mapper;

    public ParametersService(IMapper mapper, IDataAccessFacade dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    public async Task<ListWithPaginationData<TDto>> GetByParametersAsync(TParameters parameters, CancellationToken token = default)
    {
        var query = _dataAccess.GetByParameters<TRepositoryModel, TParameters>(parameters);
        var totalCount = await query.CountAsync(token);
        var resultList = await query
            .Sort(parameters.OrderBy, parameters.SortOrder)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(dto => _mapper.Map<TDto>(dto))
            .ToListAsync(token);

        return new ListWithPaginationData<TDto>(resultList, totalCount, parameters.PageNumber, parameters.PageSize);
    }
}