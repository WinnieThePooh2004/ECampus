using ECampus.Domain.Data;
using ECampus.Domain.QueryParameters;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.Interfaces;

public interface IParametersSelector<out TModel, in TParameters>
    where TModel : class, IModel
    where TParameters : IDataSelectParameters<TModel>
{
    IQueryable<TModel> SelectData(ApplicationDbContext context, TParameters parameters);
}