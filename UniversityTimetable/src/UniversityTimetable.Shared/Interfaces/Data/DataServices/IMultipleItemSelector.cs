using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices
{
    public interface IMultipleItemSelector<TModel, in TParameters>
        where TModel : class, IModel
        where TParameters : IQueryParameters<TModel>
    {
        IQueryable<TModel> SelectData(DbSet<TModel> data, TParameters parameters);
    }
}
