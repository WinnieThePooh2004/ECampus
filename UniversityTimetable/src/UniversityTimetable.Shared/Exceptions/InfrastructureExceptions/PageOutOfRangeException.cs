using System.Net;

namespace UniversityTimetable.Shared.Exceptions.InfrastructureExceptions
{
	public class PageOutOfRangeException : InfrastructureExceptions
	{
		public PageOutOfRangeException(int pageNumber, int pageSize, int totalAmount)
			: base(HttpStatusCode.BadRequest, totalAmount == 0 ?
				  $"no object found by your parameters, number can be only 1, not {pageNumber}" 
				  :
				  $"requesting page {pageNumber} with size {pageSize} requires at least {(pageNumber - 1) * pageSize} suitable objects, while your request got only {totalAmount}\n" +
				  $"maximal page number=((totalAmount+pageSize-1)/pageSize)=" +
				  $"(({totalAmount}+{pageSize - 1})/{pageSize})={(totalAmount + pageSize - 1) / pageSize}")
		{

		}
	}
}
