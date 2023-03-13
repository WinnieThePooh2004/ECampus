using System.Net;
using ECampus.Core.Extensions;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Data;
using ECampus.Domain.Exceptions.InfrastructureExceptions;
using ECampus.Domain.Requests;
using ECampus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataAccessFacades;

public class DataAccessFacade : IDataAccessFacade
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    public DataAccessFacade(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public TEntity Create<TEntity>(TEntity entity)
        where TEntity : class, IEntity
        => _serviceProvider.GetServiceOfType<IDataCreateService<TEntity>>().Create(entity, _context);

    public Task<TEntity> UpdateAsync<TEntity>(TEntity entity, CancellationToken token = default)
        where TEntity : class, IEntity
        => _serviceProvider.GetServiceOfType<IDataUpdateService<TEntity>>().UpdateAsync(entity, _context, token);

    public TEntity Delete<TEntity>(TEntity entity)
        where TEntity : class, IEntity, new()
        => _serviceProvider.GetServiceOfType<IDataDeleteService<TEntity>>().Delete(entity, _context);

    public async Task<TEntity?> GetByIdOrDefaultAsync<TEntity>(int id, CancellationToken token = default)
        where TEntity : class, IEntity
        => await _serviceProvider.GetServiceOfType<ISingleItemSelector<TEntity>>()
            .SelectModel(id, _context.Set<TEntity>(), token);

    public IQueryable<TEntity> GetByParameters<TEntity, TParameters>(TParameters parameters)
        where TEntity : class, IEntity
        where TParameters : IDataSelectParameters<TEntity> 
        => _serviceProvider.GetServiceOfType<IParametersSelector<TEntity, TParameters>>()
            .SelectData(_context, parameters);

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