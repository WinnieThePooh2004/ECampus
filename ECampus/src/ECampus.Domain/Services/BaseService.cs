﻿using AutoMapper;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain;
using Serilog;

namespace ECampus.Domain.Services;

public class BaseService<TDto, TRepositoryModel> : IBaseService<TDto>
    where TRepositoryModel : class, IModel
    where TDto : class, IDataTransferObject
{
    private readonly IBaseDataAccessFacade<TRepositoryModel> _dataAccessFacade;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public BaseService(IBaseDataAccessFacade<TRepositoryModel> dataAccessFacade, ILogger logger,
        IMapper mapper)
    {
        _dataAccessFacade = dataAccessFacade;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TDto> CreateAsync(TDto entity)
    {
        _logger.Information("Creating object of type {Type}", typeof(TDto));
        var auditory = _mapper.Map<TRepositoryModel>(entity);
        return _mapper.Map<TDto>(await _dataAccessFacade.CreateAsync(auditory));
    }

    public async Task<TDto> DeleteAsync(int? id)
    {
        _logger.Information("Deleting object of type {Type} with id={Id}", typeof(TDto), id);
        if (id is null)
        {
            throw new NullIdException();
        }
        return _mapper.Map<TDto>(await _dataAccessFacade.DeleteAsync((int)id));
    }

    public async Task<TDto> GetByIdAsync(int? id)
    {
        _logger.Information("Getting object of type {Type} with id={Id}", typeof(TDto), id);
        if (id is null)
        {
            throw new NullIdException();
        }
        return _mapper.Map<TDto>(await _dataAccessFacade.GetByIdAsync((int)id));
    }

    public async Task<TDto> UpdateAsync(TDto entity)
    {
        _logger.Information("Updating object of type {Type}", typeof(TDto));
        var auditory = _mapper.Map<TRepositoryModel>(entity);
        return _mapper.Map<TDto>(await _dataAccessFacade.UpdateAsync(auditory));
    }
}