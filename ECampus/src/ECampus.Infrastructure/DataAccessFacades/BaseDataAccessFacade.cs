using System.Net;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Interfaces.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataAccessFacades;

public class BaseDataAccessFacade<TModel> : IBaseDataAccessFacade<TModel>
    where TModel : class, IModel, new()
{
    private readonly ApplicationDbContext _context;
    private readonly ISingleItemSelector<TModel> _singleItemSelector;
    private readonly IDataUpdateService<TModel> _updateService;
    private readonly IDataCreateService<TModel> _createService;
    private readonly IDataDeleteService<TModel> _deleteService;

    public BaseDataAccessFacade(ApplicationDbContext context,
        ISingleItemSelector<TModel> singleItemSelector, IDataDeleteService<TModel> deleteService,
        IDataUpdateService<TModel> updateService, IDataCreateService<TModel> createService)
    {
        _context = context;
        _singleItemSelector = singleItemSelector;
        _deleteService = deleteService;
        _updateService = updateService;
        _createService = createService;
    }

    public async Task<TModel> CreateAsync(TModel entity)
    {
        await _createService.CreateAsync(entity, _context);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            throw new InfrastructureExceptions(HttpStatusCode.BadRequest,
                $"Error occured while saving entity of type{typeof(TModel)} details", innerException: e);
        }
        catch (Exception e)
        {
            throw new UnhandledInfrastructureException(e, entity);
        }

        return entity;
    }

    public async Task<TModel> DeleteAsync(int id)
    {
        var deletedModel = await _deleteService.DeleteAsync(id, _context);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ObjectNotFoundByIdException(typeof(TModel), id);
        }
        catch (Exception e)
        {
            throw new UnhandledInfrastructureException(e, deletedModel);
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
        catch (DbUpdateConcurrencyException)
        {
            throw new ObjectNotFoundByIdException(typeof(TModel), entity.Id);
        }
        catch (Exception e)
        {
            throw new UnhandledInfrastructureException(e, entity);
        }

        return entity;
    }
}