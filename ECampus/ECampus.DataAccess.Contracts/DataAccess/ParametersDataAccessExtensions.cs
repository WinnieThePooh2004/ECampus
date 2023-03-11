using System.Net;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.Data;
using ECampus.Domain.Exceptions.InfrastructureExceptions;
using ECampus.Domain.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.Contracts.DataAccess;

public static class ParametersDataAccessExtensions
{
    public static async Task<TEntity> GetSingleAsync<TEntity, TParameters>(
        this IDataAccessFacade parametersDataAccess, TParameters parameters, CancellationToken token = default)
        where TEntity : class, IEntity
        where TParameters : IDataSelectParameters<TEntity> 
        => await parametersDataAccess.SingleOrDefaultAsync<TEntity, TParameters>(parameters, token)
           ?? throw new InfrastructureExceptions(HttpStatusCode.NotFound,
               $"Cannot find object of type {typeof(TEntity)} by parameters of type {typeof(TParameters)}",
               parameters);

    public static async Task<TEntity> GetByIdAsync<TEntity>(
        this IDataAccessFacade dataAccessFacade, int id, CancellationToken token)
        where TEntity : class, IEntity 
        => await dataAccessFacade.GetByIdOrDefaultAsync<TEntity>(id, token) 
           ?? throw new ObjectNotFoundByIdException(typeof(TEntity), id);

    public static async Task<TEntity?> SingleOrDefaultAsync<TEntity, TParameters>(
        this IDataAccessFacade parametersDataAccess, TParameters parameters, CancellationToken token = default)
        where TEntity : class, IEntity
        where TParameters : IDataSelectParameters<TEntity> 
        => await parametersDataAccess.GetByParameters<TEntity, TParameters>(parameters).SingleOrDefaultAsync(token);

    /// <summary>
    /// wrapper method for GetByParameters with TParameters = PureByIdParameters
    /// </summary>
    /// <param name="dataAccess"></param>
    /// <param name="id">Primary key of required object</param>
    /// <param name="token">cancellation token</param>
    /// <typeparam name="TModel">Db model you are searching for</typeparam>
    /// <returns>Object without any navigation properties values</returns>
    /// <exception cref="ObjectNotFoundByIdException">When object with provided id not found</exception>
    public static async Task<TModel> PureByIdAsync<TModel>(this IDataAccessFacade dataAccess, int id,
        CancellationToken token)
        where TModel : class, IEntity
        => await dataAccess.GetByParameters<TModel, PureByIdParameters<TModel>>(new PureByIdParameters<TModel>(id))
            .SingleOrDefaultAsync(token) ?? throw new ObjectNotFoundByIdException(typeof(TModel), id);

    public static async Task<TEntity?> PureOrDefaultByIdAsync<TEntity>(this IDataAccessFacade dataAccess, int id,
        CancellationToken token)
        where TEntity : class, IEntity
        => await dataAccess.GetByParameters<TEntity, PureByIdParameters<TEntity>>(new PureByIdParameters<TEntity>(id))
            .SingleOrDefaultAsync(token);
}