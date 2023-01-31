using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.Interfaces;

public interface IDataCreateService<TModel>
    where TModel : class, IModel
{
    Task<TModel> CreateAsync(TModel model, ApplicationDbContext context);
}