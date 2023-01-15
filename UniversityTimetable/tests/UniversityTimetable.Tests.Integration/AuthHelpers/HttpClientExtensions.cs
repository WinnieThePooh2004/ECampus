using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared;

namespace UniversityTimetable.Tests.Integration.AuthHelpers;

public static class HttpClientExtensions
{
    public static void Login(this HttpClient client, User user)
    {
        var options = AuthData.DefaultOptions;
        var loginResult = new LoginResult
        {
            Email = user.Email,
            Username = user.Username,
            Role = user.Role,
            UserId = user.Id
        };
        var jwt = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: HttpContextExtensions.CreateClaims(loginResult),
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(options.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        
        loginResult.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.Token);
    }
}