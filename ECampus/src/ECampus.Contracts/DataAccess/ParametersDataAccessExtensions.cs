using System.Net;
using ECampus.Shared.Data;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Contracts.DataAccess;

public static class ParametersDataAccessExtensions
{
    public static async Task<TModel> GetSingleAsync<TModel, TParameters>(
        this IParametersDataAccessManager parametersDataAccess, TParameters parameters)
        where TModel : class, IModel
        where TParameters : IDataSelectParameters<TModel>
    {
        return await parametersDataAccess.GetByParameters<TModel, TParameters>(parameters).SingleOrDefaultAsync()
               ?? throw new InfrastructureExceptions(HttpStatusCode.NotFound,
                   $"Cannot find object of type {typeof(TModel)} by parameters of type {typeof(TParameters)}", parameters);
    }
}