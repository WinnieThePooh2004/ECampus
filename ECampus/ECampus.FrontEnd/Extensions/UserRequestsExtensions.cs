﻿using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Extensions;

namespace ECampus.FrontEnd.Extensions;

public static class UserRequestsExtensions
{
    public static async Task<UserDto> GetCurrentUserAsync(this IBaseRequests<UserDto> requests,
        IHttpContextAccessor httpContextAccessor)
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

    public static async Task<UserProfile> GetCurrentUserAsync(this IBaseRequests<UserProfile> requests,
        IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext!.User;
        if (!user.Identity!.IsAuthenticated)
        {
            throw new UnauthorizedAccessException();
        }

        return await requests.GetByIdAsync(user.GetId() ?? throw new UnauthorizedAccessException());
    }
}