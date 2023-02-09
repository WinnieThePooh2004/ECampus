using System.Net;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Shared.Data;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Contracts.DataAccess;

public static class ParametersDataAccessExtensions
{
    public static async Task<TModel> GetSingleAsync<TModel, TParameters>(
        this IDataAccessManager parametersDataAccess, TParameters parameters, CancellationToken token = default)
        where TModel : class, IModel
        where TParameters : IDataSelectParameters<TModel>
    {
        return await parametersDataAccess.GetByParameters<TModel, TParameters>(parameters).SingleOrDefaultAsync(token)
               ?? throw new InfrastructureExceptions(HttpStatusCode.NotFound,
                   $"Cannot find object of type {typeof(TModel)} by parameters of type {typeof(TParameters)}",
                   parameters);
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
    public static async Task<TModel> GetPureByIdAsync<TModel>(this IDataAccessManager dataAccess, int id,
        CancellationToken token)
        where TModel : class, IModel
    {
        return await dataAccess.GetByParameters<TModel, PureByIdParameters<TModel>>(new PureByIdParameters<TModel>(id))
            .SingleOrDefaultAsync(token) ?? throw new ObjectNotFoundByIdException(typeof(TModel), id);
    }
}