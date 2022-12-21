using UniversityTimetable.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;


namespace UniversityTimetable.Api.MiddlewareFilters
{
	public class MiddlewareExceptionFilter : IActionFilter, IOrderedFilter
	{
		private readonly ILogger<MiddlewareExceptionFilter> _logger;
		public MiddlewareExceptionFilter(ILogger<MiddlewareExceptionFilter> logger)
		{
			_logger = logger;
		}
		public int Order => int.MaxValue - 10;

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if(context.Exception is null)
			{
				return;
			}
			context.ExceptionHandled = true;
			if (context.Exception is HttpResponseException httpResponseException)
			{
				context.Result = FilterHttpResponseException(httpResponseException);
				return;
			}
			_logger.LogError(context.Exception, "Unhandled exception occured");
			context.Result = new ObjectResult(new BadResponseObject(context.Exception.Message))
			{
				StatusCode = 500
			};
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			
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
}
