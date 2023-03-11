using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Data;

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