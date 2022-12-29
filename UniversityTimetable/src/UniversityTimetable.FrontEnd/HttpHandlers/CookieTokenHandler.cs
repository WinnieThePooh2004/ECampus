using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace UniversityTimetable.FrontEnd.HttpHandlers;

public class CookieTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieTokenHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var cookie = _httpContextAccessor.HttpContext?.Request.Headers.Cookie.FirstOrDefault();
        if (cookie is null)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        request.Headers.Authorization = new AuthenticationHeaderValue(CookieAuthenticationDefaults.AuthenticationScheme, cookie);
        request.Headers.Add("_Cookie", cookie);
        return await base.SendAsync(request, cancellationToken);
    }
}