using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Shared.Data;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Exceptions.InfrastructureExceptions;

namespace ECampus.Services.Services;

public class BaseService<TDto, TRepositoryModel> : IBaseService<TDto>
    where TRepositoryModel : class, IModel, new()
    where TDto : class, IDataTransferObject
{
    private readonly IMapper _mapper;
    private readonly IDataAccessManager _dataAccess;

    public BaseService(IMapper mapper, IDataAccessManager dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    public async Task<TDto> CreateAsync(TDto entity)
    {
        var model = _mapper.Map<TRepositoryModel>(entity);
        var createdModel = await _dataAccess.CreateAsync(model);
        await _dataAccess.SaveChangesAsync();
        return _mapper.Map<TDto>(createdModel);
    }

    public async Task<TDto> DeleteAsync(int id)
    {
        var deleted = await _dataAccess.DeleteAsync<TRepositoryModel>(id);
        await _dataAccess.SaveChangesAsync();
        return _mapper.Map<TDto>(deleted);
    }

    public async Task<TDto> GetByIdAsync(int id)
    {
        var objectFromDb = await _dataAccess.GetByIdAsync<TRepositoryModel>(id);
        return _mapper.Map<TDto>(objectFromDb);
    }

    public async Task<TDto> UpdateAsync(TDto entity)
    {
        var model = _mapper.Map<TRepositoryModel>(entity);
        var updated = await _dataAccess.UpdateAsync(model);
        await _dataAccess.SaveChangesAsync();
        return _mapper.Map<TDto>(updated);
    }
}