using ECampus.Shared.Data;

namespace ECampus.Infrastructure.Interfaces;

public interface IDataDeleteService<TModel>
    where TModel : class, IModel, new()
{
    Task<TModel> DeleteAsync(int id, ApplicationDbContext context);
}