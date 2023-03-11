using ECampus.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ILogger = Serilog.ILogger;

namespace ECampus.WebApi.MiddlewareFilters;

public class MiddlewareExceptionFilter : IExceptionFilter, IOrderedFilter
{
    private readonly ILogger _logger;
    private const string ClientErrorMessage = "Something went wrong due to invalid user`s input, error code: {Code}";
    private const string ServerErrorMessage = "Some error occured in server, error code: {Code}";

    public MiddlewareExceptionFilter(ILogger logger)
    {
        _logger = logger;
    }

    public int Order => int.MaxValue - 10;

    public void OnException(ExceptionContext context)
    {
        context.ExceptionHandled = true;
        if (context.Exception is HttpResponseException httpResponseException)
        {
            context.Result = FilterHttpResponseException(httpResponseException);
            return;
        }

        _logger.Fatal(context.Exception, "Unhandled exception in unpredictable place happened," +
                                         " nobody knows what it is, all is bad :(");
        context.Result = new ObjectResult(context.Exception.Message)
        {
            StatusCode = 500
        };
    }

    private IActionResult FilterHttpResponseException(HttpResponseException exception)
    {
        LogException(exception);
        var responseObject = new BadResponseObject
        {
            Message = exception.Message,
            ResponseObject = exception.Object
        };

        return new ObjectResult(responseObject)
        {
            StatusCode = exception.StatusCode
        };
    }

    private void LogException(HttpResponseException exception)
    {
        if (exception.StatusCode <= 500)
        {
            _logger.Warning(exception, ClientErrorMessage, exception.StatusCode);
            return;
        }

        _logger.Error(exception, ServerErrorMessage, exception.StatusCode);
    }
}