using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Shared.Interfaces.Data
{
    public interface IDataSelector<TModel, in TParameters>
        where TModel : class, IModel
        where TParameters : IQueryParameters<TModel>
    {
        IQueryable<TModel> SelectData(DbSet<TModel> data, TParameters parameters);
    }
}
