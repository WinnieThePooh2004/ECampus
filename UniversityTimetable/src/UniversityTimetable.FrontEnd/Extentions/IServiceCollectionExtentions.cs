using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Extentions
{
    public static class IServiceCollectionExtentions
    {
        public static IServiceCollection AddRequests<TData, TParameters, TImplementation>(this IServiceCollection services)
            where TData : class
            where TParameters : IQueryParameters
            where TImplementation : class, IRequests<TData, TParameters>
        {
            services.AddScoped<IBaseRequests<TData>, TImplementation>();
            return services.AddScoped<IRequests<TData, TParameters>, TImplementation>();
        }

        public static IServiceCollection AddRequests<TData, TParameters>(this IServiceCollection services)
            where TData : class
            where TParameters : IQueryParameters
        {
            services.AddScoped<IBaseRequests<TData>, BaseRequests<TData>>();
            return services.AddRequests<TData, TParameters, Requests<TData, TParameters>>();
        }
    }
}
