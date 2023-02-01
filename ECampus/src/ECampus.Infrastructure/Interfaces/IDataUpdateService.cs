using ECampus.Shared.Data;

namespace ECampus.Infrastructure.Interfaces;

public interface IDataUpdateService<TModel>
    where TModel : class, IModel
{
    Task<TModel> UpdateAsync(TModel model, ApplicationDbContext context);
}