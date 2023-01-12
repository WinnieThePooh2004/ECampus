using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.HttpHandlers;

public class TokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == CustomClaimTypes.JwtBearer)?.Value;
        if (token is null)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
        return await base.SendAsync(request, cancellationToken);
    }
}