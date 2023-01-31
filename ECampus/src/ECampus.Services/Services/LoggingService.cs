﻿using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Interfaces.Domain;
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

    public async Task<T> GetByIdAsync(int? id)
    {
        _logger.Information("Retrieving object of type {Type}, with id = {Id}", typeof(T), id);
        var result = await _baseService.GetByIdAsync(id);
        _logger.Information("Successfully retrieved object of type {Type} by id = {Id}", typeof(T), id);
        return result;
    }

    public async Task<T> CreateAsync(T entity)
    {
        _logger.Information("Creating new object of type {Type}", typeof(T));
        var createdEntity = await _baseService.CreateAsync(entity);
        _logger.Information("Successfully created object of type {Type}.\n Id of new object is {Id}", typeof(T),
            entity.Id);
        return createdEntity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _logger.Information("Updating object of type {Type} with id = {Id}", typeof(T), entity.Id);
        var updatedEntity = await _baseService.UpdateAsync(entity);
        _logger.Information("Successfully updated object of type {Type} with id = {Id}", typeof(T), entity.Id);
        return updatedEntity;
    }

    public async Task<T> DeleteAsync(int? id)
    {
        _logger.Information("Deleting object of type {Type}, with id = {Id}", typeof(T), id);
        var deletedObject = await _baseService.DeleteAsync(id);
        _logger.Information("Successfully deleted object of type {Type} by id = {Id}", typeof(T), id);
        return deletedObject;
    }
}