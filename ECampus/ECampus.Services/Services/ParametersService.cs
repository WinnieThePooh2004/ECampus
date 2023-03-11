using AutoMapper;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.Data;
using ECampus.Domain.DataContainers;
using ECampus.Domain.Extensions;
using ECampus.Domain.QueryParameters;
using ECampus.Services.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services;

public class ParametersService<TDto, TParameters, TEntity> : IParametersService<TDto, TParameters>
    where TDto : class, IDataTransferObject, new()
    where TEntity : class, IEntity, new()
    where TParameters : class, IDataSelectParameters<TEntity>, IQueryParameters<TDto>
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
        var query = _dataAccess.GetByParameters<TEntity, TParameters>(parameters);
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