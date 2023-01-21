using ECampus.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;


namespace ECampus.Api.MiddlewareFilters;

public class MiddlewareExceptionFilter : IExceptionFilter, IOrderedFilter
{
	private readonly ILogger<MiddlewareExceptionFilter> _logger;
	public MiddlewareExceptionFilter(ILogger<MiddlewareExceptionFilter> logger)
	{
		_logger = logger;
	}
	
	public int Order => int.MaxValue - 10;
	
	public void OnException(ExceptionContext context)
	{
		context.ExceptionHandled = true;
		_logger.LogError(context.Exception, "Unhandled exception occured");
		if (context.Exception is HttpResponseException httpResponseException)
		{
			context.Result = FilterHttpResponseException(httpResponseException);
			return;
		}
		context.Result = new ObjectResult(context.Exception.Message)
		{
			StatusCode = 500
		};
	}
	
	private static IActionResult FilterHttpResponseException(HttpResponseException exception)
	{
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
}