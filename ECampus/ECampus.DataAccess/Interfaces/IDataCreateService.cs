using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.Interfaces;

public interface IDataCreateService<TModel>
    where TModel : class, IModel
{
    TModel Create(TModel model, ApplicationDbContext context);
}