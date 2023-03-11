using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataDeleteServices;

public class DataDeleteService<TModel> : IDataDeleteService<TModel>
    where TModel : class, IModel, new()
{
    public TModel Delete(TModel model, ApplicationDbContext context)
    {
        context.Remove(model);
        return model;
    }
}