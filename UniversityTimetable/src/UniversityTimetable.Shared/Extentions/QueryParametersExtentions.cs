using System.Web;

namespace UniversityTimetable.Shared.Extentions
{
    public static class QueryParametersExtentions
    {
        public static string ToQueryString<TParams>(this TParams parameters)
        {
            var properties = from p in typeof(TParams).GetProperties()
                             where p.GetValue(parameters, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(parameters, null).ToString());

            return string.Join("&", properties.ToArray());
        }
    }
}
