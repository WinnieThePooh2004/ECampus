﻿using AutoMapper;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.Data;
using ECampus.Services.Contracts.Services;

namespace ECampus.Services.Services;

public class BaseService<TDto, TRepositoryModel> : IBaseService<TDto>
    where TRepositoryModel : class, IModel, new()
    where TDto : class, IDataTransferObject
{
    private readonly IMapper _mapper;
    private readonly IDataAccessFacade _dataAccess;

    public BaseService(IMapper mapper, IDataAccessFacade dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    public async Task<TDto> CreateAsync(TDto entity, CancellationToken token = default)
    {
        var model = _mapper.Map<TRepositoryModel>(entity);
        var createdModel = _dataAccess.Create(model);
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<TDto>(createdModel);
    }

    public async Task<TDto> DeleteAsync(int id, CancellationToken token = default)
    {
        var deleted = _dataAccess.Delete(new TRepositoryModel { Id = id });
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<TDto>(deleted);
    }

    public async Task<TDto> GetByIdAsync(int id, CancellationToken token = default)
    {
        var objectFromDb = await _dataAccess.GetByIdAsync<TRepositoryModel>(id, token);
        return _mapper.Map<TDto>(objectFromDb);
    }

    public async Task<TDto> UpdateAsync(TDto entity, CancellationToken token = default)
    {
        var model = _mapper.Map<TRepositoryModel>(entity);
        var updated = await _dataAccess.UpdateAsync(model, token);
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<TDto>(updated);
    }
}