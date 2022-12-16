using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Extentions
{
    public static class IServiceCollectionExtentions
    {
        public static IServiceCollection AddRequests<TData, TParameters, TImplementation>(this IServiceCollection services)
            where TData : class
            where TParameters : IQueryParameters
            where TImplementation : class, IParametersIRequests<TData, TParameters>
        {
            return services.AddScoped<IParametersIRequests<TData, TParameters>, TImplementation>();
        }

        public static IServiceCollection AddRequests<TData, TParameters>(this IServiceCollection services)
            where TData : class
            where TParameters : IQueryParameters
        {
            services.AddScoped<IBaseRequests<TData>, BaseRequests<TData>>();
            return services.AddRequests<TData, TParameters, ParametersRequests<TData, TParameters>>();
        }
    }
}
