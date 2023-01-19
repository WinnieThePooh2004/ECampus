using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
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
    private readonly IDataUpdateService<TModel> _updateService;
    private readonly IDataCreateService<TModel> _createService;
    private readonly IDataDeleteService<TModel> _deleteService;

    public BaseDataAccessFacade(ApplicationDbContext context, ILogger<BaseDataAccessFacade<TModel>> logger, 
        ISingleItemSelector<TModel> singleItemSelector, IDataDeleteService<TModel> deleteService,
        IDataUpdateService<TModel> updateService, IDataCreateService<TModel> createService)
    {
        _context = context;
        _logger = logger;
        _singleItemSelector = singleItemSelector;
        _deleteService = deleteService;
        _updateService = updateService;
        _createService = createService;
    }

    public async Task<TModel> CreateAsync(TModel entity)
    {
        await _createService.CreateAsync(entity, _context);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<TModel> DeleteAsync(int id)
    {
        var deletedModel = await _deleteService.DeleteAsync(id, _context);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError(e, "Db update to successful");
            throw new ObjectNotFoundByIdException(typeof(TModel), id);
        }
        return deletedModel;
    }

    public async Task<TModel> GetByIdAsync(int id)
    {
        var model = await _singleItemSelector.SelectModel(id, _context.Set<TModel>())
            ?? throw new ObjectNotFoundByIdException(typeof(TModel), id);
        return model;
    }

    public async Task<TModel> UpdateAsync(TModel entity)
    {
        await _updateService.UpdateAsync(entity, _context);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError(e, "Db update was not successful");
            throw new ObjectNotFoundByIdException(typeof(TModel), entity.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Db update was not successful");
            throw new InfrastructureExceptions(HttpStatusCode.InternalServerError, e.ToString());
        }

        return entity;
    }
}