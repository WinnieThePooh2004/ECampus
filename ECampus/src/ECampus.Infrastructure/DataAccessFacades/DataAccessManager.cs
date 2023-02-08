using System.Net;
using ECampus.Contracts.DataAccess;
using ECampus.Core.Extensions;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Data;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataAccessFacades;

public class DataAccessManager : IDataAccessManager
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    public DataAccessManager(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public Task<TModel> CreateAsync<TModel>(TModel model, CancellationToken token = default)
        where TModel : class, IModel
    {
        var service = _serviceProvider.GetServiceOfType<IDataCreateService<TModel>>();
        return service.CreateAsync(model, _context, token);
    }

    public Task<TModel> UpdateAsync<TModel>(TModel model, CancellationToken token = default)
        where TModel : class, IModel
    {
        return _serviceProvider.GetServiceOfType<IDataUpdateService<TModel>>().UpdateAsync(model, _context, token);
    }

    public Task<TModel> DeleteAsync<TModel>(int id, CancellationToken token = default)
        where TModel : class, IModel, new()
    {
        return _serviceProvider.GetServiceOfType<IDataDeleteService<TModel>>().DeleteAsync(id, _context, token);
    }

    public async Task<TModel> GetByIdAsync<TModel>(int id, CancellationToken token = default)
        where TModel : class, IModel
    {
        return await _serviceProvider.GetServiceOfType<ISingleItemSelector<TModel>>()
                   .SelectModel(id, _context.Set<TModel>(), token)
               ?? throw new ObjectNotFoundByIdException(typeof(TModel), id);
    }

    public IQueryable<TModel> GetByParameters<TModel, TParameters>(TParameters parameters)
        where TModel : class, IModel
        where TParameters : IDataSelectParameters<TModel>
    {
        var selector = _serviceProvider.GetServiceOfType<IMultipleItemSelector<TModel, TParameters>>();
        return selector.SelectData(_context, parameters);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken token = default)
    {
        try
        {
            return await _context.SaveChangesAsync(token) > 0;
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new InfrastructureExceptions(HttpStatusCode.NotFound,
                "Error occured while saving changes to db\n" +
                "Please, check is object(s) you are trying to update/delete exist", innerException: e);
        }
        catch (DbUpdateException e)
        {
            throw new InfrastructureExceptions(HttpStatusCode.BadRequest,
                "Error occured while saving changes to db\n" +
                "Please, ensure that all FK of object are valid and all non-nullable properties are not null",
                innerException: e);
        }
        catch (Exception e)
        {
            throw new UnhandledInfrastructureException(e);
        }
    }
}