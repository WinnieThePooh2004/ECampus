using System.Net;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.DataAccess;

namespace UniversityTimetable.Infrastructure.DataAccessFacades;

public class BaseDataAccessFacade<TModel> : IBaseDataAccessFacade<TModel>
    where TModel : class, IModel, new()
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BaseDataAccessFacade<TModel>> _logger;
    private readonly ISingleItemSelector<TModel> _singleItemSelector;
    private readonly IDataUpdate<TModel> _update;
    private readonly IDataCreate<TModel> _create;
    private readonly IDataDelete<TModel> _delete;

    public BaseDataAccessFacade(ApplicationDbContext context, ILogger<BaseDataAccessFacade<TModel>> logger, 
        ISingleItemSelector<TModel> singleItemSelector, IDataDelete<TModel> delete,
        IDataUpdate<TModel> update, IDataCreate<TModel> create)
    {
        _context = context;
        _logger = logger;
        _singleItemSelector = singleItemSelector;
        _delete = delete;
        _update = update;
        _create = create;
    }

    public async Task<TModel> CreateAsync(TModel entity)
    {
        await _create.CreateAsync(entity, _context);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        await _delete.DeleteAsync(id, _context);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Db update to successful");
            _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(TModel), id));
        }
    }

    public async Task<TModel> GetByIdAsync(int id)
    {
        var model = await _singleItemSelector.SelectModel(id, _context.Set<TModel>());
        if (model is null)
        {
            _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(TModel), id));
        }
        return model;
    }

    public async Task<TModel> UpdateAsync(TModel entity)
    {
        await _update.UpdateAsync(entity, _context);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Db update was not successful");
            _logger.LogAndThrowException(new InfrastructureExceptions(HttpStatusCode.BadRequest, e.Message));
        }
        return entity;
    }
}