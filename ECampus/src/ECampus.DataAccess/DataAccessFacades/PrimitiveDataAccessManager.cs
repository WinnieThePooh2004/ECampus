using System.Net;
using ECampus.Contracts.DataAccess;
using ECampus.DataAccess.DataSelectors.SingleItemSelectors;
using ECampus.Infrastructure;
using ECampus.Shared.Data;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataAccessFacades;

public class PrimitiveDataAccessManager : IDataAccessManager
{
    private readonly ApplicationDbContext _context;

    public PrimitiveDataAccessManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<TModel> CreateAsync<TModel>(TModel model, CancellationToken token = default) where TModel : class, IModel
    {
        _context.Update(model);
        return Task.FromResult(model);
    }

    public Task<TModel> UpdateAsync<TModel>(TModel model, CancellationToken token = default) where TModel : class, IModel
    {
        _context.Update(model);
        return Task.FromResult(model);
    }

    public Task<TModel> DeleteAsync<TModel>(int id, CancellationToken token = default) where TModel : class, IModel, new()
    {
        var model = new TModel { Id = id };
        _context.Update(model);
        return Task.FromResult(model);
    }

    public async Task<TModel> GetByIdAsync<TModel>(int id, CancellationToken token = default) where TModel : class, IModel
    {
        return await _context.Set<TModel>().GetPureByIdAsync(id, token: token) ??
               throw new ObjectNotFoundByIdException(typeof(TModel), id);
    }

    public IQueryable<TModel> GetByParameters<TModel, TParameters>(TParameters parameters) 
        where TModel : class, IModel 
        where TParameters : IDataSelectParameters<TModel>
    {
        return _context.Set<TModel>();
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