using System.Net;

namespace UniversityTimetable.Shared.Exceptions.InfrastructureExceptions
{
	public class InfrastructureExceptions : HttpResponseExeption
	{
		public InfrastructureExceptions(HttpStatusCode statusCode, string message = "", object @object = null) : base(statusCode, message, @object)
		{
		}
	}
}
