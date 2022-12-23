using UniversityTimetable.Domain.Services;
using UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;
using UniversityTimetable.Infrastructure.Repositories;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Api.Extensions;

public static class IServiceCollectionExtensions
{        
    public static IServiceCollection AddMultipleDataSelector<TModel, TParameters, TImplementation>(this IServiceCollection services)
        where TModel : class, IModel
        where TParameters : IQueryParameters<TModel>
        where TImplementation : class, IMultipleItemSelector<TModel, TParameters>
        => services.AddScoped<IMultipleItemSelector<TModel, TParameters>, TImplementation>();

    public static IServiceCollection AddDefaultSingleItemSelector<TModel>(this IServiceCollection services)
        where TModel : class, IModel
        => services.AddScoped<ISingleItemSelector<TModel>, SingleItemSelector<TModel>>();
    
    public static IServiceCollection AddDefaultServices<TModel, TDto, TParameters>(this IServiceCollection services)
        where TParameters : class, IQueryParameters<TModel>, new()
        where TModel : class, IModel, new()
        where TDto : class, IDataTransferObject, new()
    {
        services.AddScoped<IBaseRepository<TModel>, BaseRepository<TModel>>();
        services.AddScoped<IParametersService<TDto, TParameters>, ParametersService<TDto, TParameters, TModel>>();
        services.AddScoped<IBaseService<TDto>, BaseService<TDto, TModel>>();
        services.AddScoped<IParametersRepository<TModel, TParameters>, ParametersRepository<TModel, TParameters>>();
        return services;
    }
}