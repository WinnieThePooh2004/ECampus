using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Tests.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ECampus.Tests.Integration.AuthHelpers;

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

    public static void Login(this HttpClient client, UserRole role) => client.Login(DefaultUsers.GetUserByRole(role));
}