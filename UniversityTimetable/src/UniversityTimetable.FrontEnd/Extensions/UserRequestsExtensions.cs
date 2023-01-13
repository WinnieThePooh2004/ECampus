using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.Extensions;

public static class UserRequestsExtensions
{
    public static async Task<UserDto> GetCurrentUserAsync(this IBaseRequests<UserDto> requests, IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext?.User.Identity is null)
        {
            throw new UnauthorizedAccessException();
        }
        var user = httpContextAccessor.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException();
        }
        
        return await requests.GetByIdAsync(user.GetId() ?? throw new UnauthorizedAccessException());
    }
}