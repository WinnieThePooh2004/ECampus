namespace ECampus.Api.MiddlewareFilters;

public class BadResponseObject
{
	public string Message { get; init; } = string.Empty;
	public object? ResponseObject { get; init; }
}