using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Shared.Data;
using ECampus.Shared.DataContainers;
using ECampus.Shared.Extensions;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services;

public class ParametersService<TDto, TParameters, TRepositoryModel> : IParametersService<TDto, TParameters>
    where TDto : class, IDataTransferObject, new()
    where TRepositoryModel : class, IModel, new()
    where TParameters : class, IDataSelectParameters<TRepositoryModel>, IQueryParameters
{
    private readonly IDataAccessManager _dataAccess;
    private readonly IMapper _mapper;

    public ParametersService(IMapper mapper, IDataAccessManager dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    public async Task<ListWithPaginationData<TDto>> GetByParametersAsync(TParameters parameters)
    {
        var query = _dataAccess.GetByParameters<TRepositoryModel, TParameters>(parameters);
        var totalCount = await query.CountAsync();
        var resultList = await query
            .Sort(parameters.OrderBy, parameters.SortOrder)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(dto => _mapper.Map<TDto>(dto))
            .ToListAsync();

        return new ListWithPaginationData<TDto>(resultList, totalCount, parameters.PageNumber, parameters.PageSize);
    }
}