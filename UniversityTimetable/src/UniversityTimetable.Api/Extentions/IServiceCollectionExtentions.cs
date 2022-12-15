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
        public static IServiceCollection AddRepositoryWithService<TModel, TDTO, TParameters, 
                TRepositoryImplementation, TServiceImplementation>
                (this IServiceCollection services)
            where TModel : class, IModel
            where TDTO : class, IDataTransferObject
            where TParameters : class, IQueryParameters<TModel>
            where TServiceImplementation : class, IService<TDTO, TParameters>
            where TRepositoryImplementation : class, IRepository<TModel, TParameters>
            => services.AddScoped<IService<TDTO, TParameters>, TServiceImplementation>()
                .AddScoped<IRepository<TModel, TParameters>, TRepositoryImplementation>();

        public static IServiceCollection AddRepositoryWithDefaultService<TModel, TDTO, TParameters, TRepositoryImplementation>
            (this IServiceCollection services)
            where TModel : class, IModel, new()
            where TDTO : class, IDataTransferObject, new()
            where TParameters : class, IQueryParameters<TModel>
            where TRepositoryImplementation : class, IRepository<TModel, TParameters>
            => services.AddRepositoryWithService<TModel, TDTO, TParameters, TRepositoryImplementation, Service<TDTO, TParameters, TModel>>()
            .AddScoped<IBaseService<TDTO>, BaseService<TDTO, TModel>>();

        public static IServiceCollection AddDefaultRepositoryWithService<TModel, TDTO, TParameters,
        TServiceImplementation>
            (this IServiceCollection services)
            where TModel : class, IModel, new()
            where TDTO : class, IDataTransferObject
            where TParameters : class, IQueryParameters<TModel>
            where TServiceImplementation : class, IService<TDTO, TParameters>
            => services.AddRepositoryWithService<TModel, TDTO, TParameters, Repository<TModel, TParameters>, TServiceImplementation>()
            .AddScoped<IBaseRepository<TModel>, BaseRepository<TModel>>();

        public static IServiceCollection AddDefaultRepositoryWithDefaultService<TModel, TDTO, TParameters>(this IServiceCollection services)
            where TModel : class, IModel, new()
            where TDTO : class, IDataTransferObject, new()
            where TParameters : class, IQueryParameters<TModel>
            => services.AddRepositoryWithService<TModel, TDTO, TParameters, Repository<TModel, TParameters>, Service<TDTO, TParameters, TModel>>()
            .AddScoped<IBaseService<TDTO>, BaseService<TDTO, TModel>>()
            .AddScoped<IBaseRepository<TModel>, BaseRepository<TModel>>();

        public static IServiceCollection AddDataSelector<TModel, TParameters, TImplementation>(this IServiceCollection services)
            where TModel : class, IModel
            where TParameters : IQueryParameters<TModel>
            where TImplementation : class, IDataSelector<TModel, TParameters>
            => services.AddScoped<IDataSelector<TModel, TParameters>, TImplementation>();
            
    }
}
