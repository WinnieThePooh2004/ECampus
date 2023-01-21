using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Shared.Interfaces.Data.DataServices;

public interface IMultipleItemSelector<TModel, in TParameters>
    where TModel : class, IModel
    where TParameters : IQueryParameters<TModel>
{
    IQueryable<TModel> SelectData(DbSet<TModel> data, TParameters parameters);
}