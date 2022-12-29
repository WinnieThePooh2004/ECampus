using System.Web;

namespace UniversityTimetable.Shared.Extensions;

public static class QueryParametersExtensions
{
    public static string ToQueryString<TParams>(this TParams parameters)
    {
        var properties = parameters?.GetType().GetProperties()
            .Select(p => $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(parameters, null)?.ToString() ?? " ")}")
            .ToList() ?? new List<string>();
        return string.Join("&", properties);
    }
}