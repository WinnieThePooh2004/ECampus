using ECampus.Infrastructure;
using ECampus.Shared.Data;

namespace ECampus.DataAccess.Interfaces;

public interface IDataCreateService<TModel>
    where TModel : class, IModel
{
    TModel Create(TModel model, ApplicationDbContext context);
}