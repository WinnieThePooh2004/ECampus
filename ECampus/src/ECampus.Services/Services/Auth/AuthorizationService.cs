using System.IdentityModel.Tokens.Jwt;
using System.Net;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Core.Installers;
using ECampus.Domain.Interfaces.Auth;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace ECampus.Services.Services.Auth;

[Inject(typeof(IAuthorizationService))]
public class AuthorizationService : IAuthorizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtAuthOptions _authOptions;
    private readonly IDataAccessManager _parametersDataAccess;

    public AuthorizationService(IHttpContextAccessor httpContextAccessor, JwtAuthOptions authOptions,
        IDataAccessManager parametersDataAccess)
    {
        _httpContextAccessor = httpContextAccessor;
        _authOptions = authOptions;
        _parametersDataAccess = parametersDataAccess;
    }

    public async Task<LoginResult> Login(LoginDto login, CancellationToken token = default)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new HttpContextNotFoundExceptions();
        }

        var user = await _parametersDataAccess.GetSingleAsync<User, UserEmailParameters>(new UserEmailParameters
            { Email = login.Email }, token: token);
        if (user.Password != login.Password)
        {
            throw new DomainException(HttpStatusCode.BadRequest, "Wrong password or email");
        }

        var result = new LoginResult
        {
            Email = user.Email,
            Role = user.Role.ToString(),
            Username = user.Username,
            UserId = user.Id,
            StudentId = user.StudentId,
            TeacherId = user.TeacherId,
            GroupId = user.Student?.GroupId
        };

        var jwt = new JwtSecurityToken(
            issuer: _authOptions.Issuer,
            audience: _authOptions.Audience,
            claims: HttpContextExtensions.CreateClaims(result),
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
            signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        result.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return result;
    }
}