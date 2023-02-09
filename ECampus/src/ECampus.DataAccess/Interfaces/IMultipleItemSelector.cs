using ECampus.Infrastructure;
using ECampus.Shared.Data;
using ECampus.Shared.QueryParameters;

namespace ECampus.DataAccess.Interfaces;

public interface IMultipleItemSelector<out TModel, in TParameters>
    where TModel : class, IModel
    where TParameters : IDataSelectParameters<TModel>
{
    IQueryable<TModel> SelectData(ApplicationDbContext context, TParameters parameters);
}