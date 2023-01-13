using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequests<TData, TParameters>(this IServiceCollection services)
        where TData : class
        where TParameters : IQueryParameters
    {
        services.AddScoped<IBaseRequests<TData>, BaseRequests<TData>>();
        return services.AddScoped<IParametersRequests<TData, TParameters>, ParametersRequests<TData, TParameters>>();
    }
}