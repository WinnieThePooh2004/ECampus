using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataCreateServices;

public class DataCreateService<TModel> : IDataCreateService<TModel> 
    where TModel : class, IModel, new()
{
    public TModel Create(TModel model, ApplicationDbContext context)
    {
        context.Add(model);
        return model;
    }
}