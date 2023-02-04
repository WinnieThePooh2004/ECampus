using ECampus.Shared.Data;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataAccess;

public interface IParametersDataAccessManager
{
    IQueryable<TModel> GetByParameters<TModel, TParameters>(TParameters parameters)
        where TModel : class, IModel
        where TParameters : IDataSelectParameters<TModel>;
}