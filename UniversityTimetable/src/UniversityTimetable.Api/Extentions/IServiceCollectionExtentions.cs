using UniversityTimetable.Domain.Services;
using UniversityTimetable.Infrastructure.Repositories;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Api.Extentions
{
    public static class IServiceCollectionExtentions
    {        
        public static IServiceCollection AddDataSelector<TModel, TParameters, TImplementation>(this IServiceCollection services)
            where TModel : class, IModel
            where TParameters : IQueryParameters<TModel>
            where TImplementation : class, IDataSelector<TModel, TParameters>
            => services.AddScoped<IDataSelector<TModel, TParameters>, TImplementation>();

        public static IServiceCollection AddDefaultServices<TModel, TDTO, TParameters>(this IServiceCollection services)
            where TParameters : class, IQueryParameters<TModel>, new()
            where TModel : class, IModel, new()
            where TDTO : class, IDataTransferObject, new()
        {
            services.AddScoped<IService<TDTO, TParameters>, Service<TDTO, TParameters, TModel>>();
            services.AddScoped<IBaseService<TDTO>, BaseService<TDTO, TModel>>();
            services.AddScoped<IRepository<TModel, TParameters>, Repository<TModel, TParameters>>();
            services.AddScoped<IBaseRepository<TModel>, BaseRepository<TModel>>();
            return services;
        }
            
    }
}
