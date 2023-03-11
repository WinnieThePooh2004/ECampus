using System.Net;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Shared.Data;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.Contracts.DataAccess;

public static class ParametersDataAccessExtensions
{
    public static async Task<TModel> GetSingleAsync<TModel, TParameters>(
        this IDataAccessFacade parametersDataAccess, TParameters parameters, CancellationToken token = default)
        where TModel : class, IModel
        where TParameters : IDataSelectParameters<TModel>
    {
        return await parametersDataAccess.SingleOrDefaultAsync<TModel, TParameters>(parameters, token)
               ?? throw new InfrastructureExceptions(HttpStatusCode.NotFound,
                   $"Cannot find object of type {typeof(TModel)} by parameters of type {typeof(TParameters)}",
                   parameters);
    }

    public static async Task<TModel?> SingleOrDefaultAsync<TModel, TParameters>(
        this IDataAccessFacade parametersDataAccess, TParameters parameters, CancellationToken token = default)
        where TModel : class, IModel
        where TParameters : IDataSelectParameters<TModel>
    {
        return await parametersDataAccess.GetByParameters<TModel, TParameters>(parameters).SingleOrDefaultAsync(token);
    }

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
        where TModel : class, IModel
    {
        return await dataAccess.GetByParameters<TModel, PureByIdParameters<TModel>>(new PureByIdParameters<TModel>(id))
            .SingleOrDefaultAsync(token) ?? throw new ObjectNotFoundByIdException(typeof(TModel), id);
    }

    public static async Task<TModel?> PureOrDefaultByIdAsync<TModel>(this IDataAccessFacade dataAccess, int id,
        CancellationToken token)
        where TModel : class, IModel
    {
        return await dataAccess.GetByParameters<TModel, PureByIdParameters<TModel>>(new PureByIdParameters<TModel>(id))
            .SingleOrDefaultAsync(token);
    }
}