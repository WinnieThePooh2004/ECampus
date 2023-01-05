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

namespace UniversityTimetable.Domain.Auth;

public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationDataAccess _repository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationService(IAuthorizationDataAccess repository,
        IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
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
            UserId = user.Id,
        };
        
        var jwt = new JwtSecurityToken(
            issuer: JwtAuthOptions.Issuer,
            audience: JwtAuthOptions.Audience,
            claims: HttpContextExtensions.CreateClaims(result),
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(JwtAuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        
        result.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return result;
    }

    public Task Logout()
    {
        return Task.CompletedTask;
    }
}