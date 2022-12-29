namespace UniversityTimetable.Api.MiddlewareFilters
{
	public class BadResponseObject
	{
		public string Message { get; set; } = string.Empty;
		public object? ResponseObject { get; init; }

		public BadResponseObject(string message, object? responseObject = null)
		{
			Message = message;
			ResponseObject = responseObject;
		}

		public BadResponseObject()
		{
			
		}
	}
}
