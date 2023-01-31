using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.Interfaces;

public interface IMultipleItemSelector<TModel, in TParameters>
    where TModel : class, IModel
    where TParameters : IQueryParameters<TModel>
{
    IQueryable<TModel> SelectData(DbSet<TModel> data, TParameters parameters);
}