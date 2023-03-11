using System.Web;

namespace ECampus.Domain.Extensions;

public static class QueryParametersExtensions
{
    public static string ToQueryString<TParams>(this TParams parameters)
    {
        var properties
            = parameters?.GetType().GetProperties()
                  .Select(p =>
                      $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(parameters, null)?.ToQueryStringParamValue())}") ??
              Array.Empty<string>();
        return string.Join("&", properties);
    }

    private static string ToQueryStringParamValue(this object? obj)
    {
        if (obj is null)
        {
            return string.Empty;
        }

        var type = obj.GetType();
        return type == typeof(DateTime) ? ((DateTime)obj).ToString("yyyy-MM-ddTHH:mm:ss") : obj.ToString()!;
    }
}