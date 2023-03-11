using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.Interfaces;

public interface IDataDeleteService<TModel>
    where TModel : class, IModel
{
    TModel Delete(TModel model, ApplicationDbContext context);
}