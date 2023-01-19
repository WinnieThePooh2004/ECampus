using System.IdentityModel.Tokens.Jwt;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Metadata;

namespace UniversityTimetable.Domain.Auth;

[Inject(typeof(IAuthorizationService))]
public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationDataAccess _repository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtAuthOptions _authOptions;

    public AuthorizationService(IAuthorizationDataAccess repository,
        IMapper mapper, IHttpContextAccessor httpContextAccessor, JwtAuthOptions authOptions)
    {
        _repository = repository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _authOptions = authOptions;
    }

    public async Task<LoginResult> Login(LoginDto login)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new HttpContextNotFoundExceptions();
        }

        var user = _mapper.Map<UserDto>(await _repository.GetByEmailAsync(login.Email));
        if (user.Password != login.Password)
        {
            throw new DomainException(HttpStatusCode.BadRequest, "Wrong password or email");
        }

        var result = new LoginResult
        {
            Email = user.Email,
            Role = user.Role,
            Username = user.Username,
            UserId = user.Id
        };
        
        var jwt = new JwtSecurityToken(
            issuer: _authOptions.Issuer,
            audience: _authOptions.Audience,
            claims: HttpContextExtensions.CreateClaims(result),
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        
        result.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return result;
    }
}