using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static IServiceCollection AddRequests<TData, TParameters, TImplementation>(this IServiceCollection services)
            where TData : class
            where TParameters : IQueryParameters
            where TImplementation : class, IParametersRequests<TData, TParameters>
        {
            return services.AddScoped<IParametersRequests<TData, TParameters>, TImplementation>();
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
