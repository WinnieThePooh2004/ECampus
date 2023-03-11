using System.Net;

namespace ECampus.Domain.Exceptions;

public class HttpResponseException : Exception
{
    public int StatusCode{ get; }
    public object? Object { get; }

    public HttpResponseException(HttpStatusCode statusCode, string message = "", object? @object = null, Exception? innerException = null)
        :base(message, innerException)
    {
        StatusCode = (int)statusCode;
        Object = @object;
    }

    public override string Message 
        => base.Message + $"\nError code: {StatusCode}";
}