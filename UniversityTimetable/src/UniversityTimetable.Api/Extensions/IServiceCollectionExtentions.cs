using UniversityTimetable.Domain.Services;
using UniversityTimetable.Domain.Validation.CreateValidators;
using UniversityTimetable.Domain.Validation.UpdateValidators;
using UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;
using UniversityTimetable.Infrastructure.DataAccessFacades;
using UniversityTimetable.Infrastructure.DataCreateServices;
using UniversityTimetable.Infrastructure.DataDeleteServices;
using UniversityTimetable.Infrastructure.DataUpdateServices;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Api.Extensions;

// ReSharper disable once InconsistentNaming
internal static class IServiceCollectionExtensions
{        
    public static IServiceCollection AddMultipleDataSelector<TModel, TParameters, TImplementation>(this IServiceCollection services)
        where TModel : class, IModel
        where TParameters : IQueryParameters<TModel>
        where TImplementation : class, IMultipleItemSelector<TModel, TParameters>
        => services.AddSingleton<IMultipleItemSelector<TModel, TParameters>, TImplementation>();
    
    public static IServiceCollection AddDefaultFacadeServices<TModel, TDto, TParameters>(this IServiceCollection services)
        where TParameters : class, IQueryParameters<TModel>, new()
        where TModel : class, IModel, new()
        where TDto : class, IDataTransferObject, new() =>
        services.AddScoped<IBaseDataAccessFacade<TModel>, BaseDataAccessFacade<TModel>>()
            .AddScoped<IParametersService<TDto, TParameters>, ParametersService<TDto, TParameters, TModel>>()
            .AddScoped<IBaseService<TDto>, BaseService<TDto, TModel>>()
            .AddScoped<IParametersDataAccessFacade<TModel, TParameters>, ParametersDataAccessFacade<TModel, TParameters>>();

    public static IServiceCollection AddDefaultDataServices<TModel>(this IServiceCollection services)
        where TModel : class, IModel, new()
        => services.AddSingleton<IDataUpdateService<TModel>, DataUpdateService<TModel>>()
            .AddSingleton<IDataDeleteService<TModel>, DataDeleteService<TModel>>()
            .AddSingleton<IDataCreateService<TModel>, DataCreateService<TModel>>()
            .AddSingleton<ISingleItemSelector<TModel>, SingleItemSelector<TModel>>();

    public static IServiceCollection AddDefaultDomainServices<TDto>(this IServiceCollection services)
        where TDto : class, IDataTransferObject, new()
        => services.AddScoped<IUpdateValidator<TDto>, UpdateValidator<TDto>>()
            .AddScoped<ICreateValidator<TDto>, CreateValidator<TDto>>();
}