using ECampus.FrontEnd.Requests;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.QueryParameters;

namespace ECampus.FrontEnd.Extensions;

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