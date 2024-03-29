﻿using ECampus.Domain.Data;
using ECampus.Services.Contracts.Services;
using Serilog;

namespace ECampus.Services.Services;

public class LoggingService<T> : IBaseService<T>
    where T : class, IDataTransferObject
{
    private readonly IBaseService<T> _baseService;
    private readonly ILogger _logger;

    public LoggingService(IBaseService<T> baseService, ILogger logger)
    {
        _baseService = baseService;
        _logger = logger;
    }

    public async Task<T> GetByIdAsync(int id, CancellationToken token = default)
    {
        _logger.Information("Retrieving object of type {Type}, with id = {Id}", typeof(T), id);
        var result = await _baseService.GetByIdAsync(id, token);
        _logger.Information("Successfully retrieved object of type {Type} by id = {Id}", typeof(T), id);
        return result;
    }

    public async Task<T> CreateAsync(T dto, CancellationToken token = default)
    {
        _logger.Information("Creating new object of type {Type}", typeof(T));
        var createdEntity = await _baseService.CreateAsync(dto, token);
        _logger.Information("Successfully created object of type {Type}.\n Id of new object is {Id}", typeof(T),
            dto.Id);
        return createdEntity;
    }

    public async Task<T> UpdateAsync(T dto, CancellationToken token = default)
    {
        _logger.Information("Updating object of type {Type} with id = {Id}", typeof(T), dto.Id);
        var updatedEntity = await _baseService.UpdateAsync(dto, token);
        _logger.Information("Successfully updated object of type {Type} with id = {Id}", typeof(T), dto.Id);
        return updatedEntity;
    }

    public async Task<T> DeleteAsync(int id, CancellationToken token = default)
    {
        _logger.Information("Deleting object of type {Type}, with id = {Id}", typeof(T), id);
        var deletedObject = await _baseService.DeleteAsync(id, token);
        _logger.Information("Successfully deleted object of type {Type} by id = {Id}", typeof(T), id);
        return deletedObject;
    }
}