using System.Net;

namespace UniversityTimetable.Shared.Exceptions
{
    public class HttpResponseExeption : Exception
    {
        public int StatusCode{ get; }
        public object Object { get; }
        public HttpResponseExeption(HttpStatusCode statusCode, string message = "", object @object = null)
            :base(message)
        {
            StatusCode = (int)statusCode;
            Object = @object;
        }

        public override string Message 
            => base.Message + $"\nError code: {StatusCode}";
    }
}
