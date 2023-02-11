using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static IHttpContextAccessor CreateContextAccessor(this ClaimsPrincipal user)
    {
        var result = Substitute.For<IHttpContextAccessor>();
        result.HttpContext = Substitute.For<HttpContext>();
        result.HttpContext.User = user;
        return result;
    }
}