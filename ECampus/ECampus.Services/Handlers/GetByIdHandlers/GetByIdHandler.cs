using AutoMapper;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.Data;
using ECampus.Domain.Responses;
using ECampus.Services.Interfaces;

namespace ECampus.Services.Handlers.GetByIdHandlers;

public class GetByIdHandler<TResponse, TEntity> : IGetByIdHandler<TResponse> 
    where TResponse : ISingleItemResponse<TEntity>
    where TEntity : class, IEntity
{
    private readonly IMapper _mapper;
    private readonly IDataAccessFacade _dataAccess;

    public GetByIdHandler(IMapper mapper, IDataAccessFacade dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    public async Task<TResponse> GetByIdAsync(int id, CancellationToken token)
    {
        var entity = await _dataAccess.GetByIdAsync<TEntity>(id, token);
        return _mapper.Map<TResponse>(entity);
    }
}