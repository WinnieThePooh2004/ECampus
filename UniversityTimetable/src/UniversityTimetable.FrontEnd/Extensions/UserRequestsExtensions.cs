using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.Extensions;

public static class UserRequestsExtensions
{
    public static async Task<UserDto> GetCurrentUserAsync(this IBaseRequests<UserDto> requests, IHttpContextAccessor httpContextAccessor)
    {
        return await requests.GetByIdAsync(httpContextAccessor.HttpContext?.User.GetId() ?? throw new Exception());
    }
}