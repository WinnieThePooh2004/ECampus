using System.Net;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;

namespace UniversityTimetable.Infrastructure.Repositories;

public class BaseRepository<TModel> : IBaseRepository<TModel>
    where TModel : class, IModel, new()
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BaseRepository<TModel>> _logger;
    private readonly ISingleItemSelector<TModel> _singleItemSelector;

    public BaseRepository(ApplicationDbContext context, ILogger<BaseRepository<TModel>> logger, ISingleItemSelector<TModel> singleItemSelector)
    {
        _context = context;
        _logger = logger;
        _singleItemSelector = singleItemSelector;
    }

    public async Task<TModel> CreateAsync(TModel entity)
    {
        if (entity.Id != 0)
        {
            _logger.LogAndThrowException(new InfrastructureExceptions(HttpStatusCode.BadRequest,
                "Cannot create add object to db if id != 0"));
        }
        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var model = new TModel { Id = id };
        _context.Remove(model);
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
        _context.Update(entity);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Db update to successful");
            _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(TModel), entity.Id));
        }
        return entity;
    }
}