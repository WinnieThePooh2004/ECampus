using ECampus.Infrastructure;
using ECampus.Shared.Data;

namespace ECampus.DataAccess.Interfaces;

public interface IDataDeleteService<TModel>
    where TModel : class, IModel
{
    TModel Delete(TModel model, ApplicationDbContext context);
}