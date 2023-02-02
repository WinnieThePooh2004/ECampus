using ECampus.Shared.Data;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.Interfaces;

public interface IMultipleItemSelector<out TModel, in TParameters>
    where TModel : class, IModel
    where TParameters : IQueryParameters<TModel>
{
    IQueryable<TModel> SelectData(ApplicationDbContext context, TParameters parameters);
}