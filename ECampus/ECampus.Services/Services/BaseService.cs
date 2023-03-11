using AutoMapper;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.Data;
using ECampus.Services.Contracts.Services;

namespace ECampus.Services.Services;

public class BaseService<TDto, TEntity> : IBaseService<TDto>
    where TEntity : class, IEntity, new()
    where TDto : class, IDataTransferObject
{
    private readonly IMapper _mapper;
    private readonly IDataAccessFacade _dataAccess;

    public BaseService(IMapper mapper, IDataAccessFacade dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    public async Task<TDto> CreateAsync(TDto dto, CancellationToken token = default)
    {
        var entity = _mapper.Map<TEntity>(dto);
        var createdModel = _dataAccess.Create(entity);
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<TDto>(createdModel);
    }

    public async Task<TDto> DeleteAsync(int id, CancellationToken token = default)
    {
        var deleted = _dataAccess.Delete(new TEntity { Id = id });
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<TDto>(deleted);
    }

    public async Task<TDto> GetByIdAsync(int id, CancellationToken token = default)
    {
        var objectFromDb = await _dataAccess.GetByIdAsync<TEntity>(id, token);
        return _mapper.Map<TDto>(objectFromDb);
    }

    public async Task<TDto> UpdateAsync(TDto dto, CancellationToken token = default)
    {
        var model = _mapper.Map<TEntity>(dto);
        var updated = await _dataAccess.UpdateAsync(model, token);
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<TDto>(updated);
    }
}